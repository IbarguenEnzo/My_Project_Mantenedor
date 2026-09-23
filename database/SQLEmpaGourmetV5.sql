/*CREATE DATABASE EmpaGourmet;
GO
USE EmpaGourmet;
GO



CREATE TABLE ROL(
    IdRol INT IDENTITY (1,1) PRIMARY KEY,
    Descripcion VARCHAR(50) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE()
);

CREATE TABLE USUARIO(
    IdUsuario INT IDENTITY (1,1) PRIMARY KEY,
    Documento VARCHAR(50) NOT NULL,
    NombreCompleto VARCHAR(100) NOT NULL,
    Correo VARCHAR(50) NOT NULL,
    Contrasena VARCHAR(50) NOT NULL,
    IdRol INT NOT NULL,
    Estado BIT DEFAULT 1,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Usuario_Rol FOREIGN KEY (IdRol) REFERENCES ROL(IdRol)
);

CREATE TABLE CATEGORIA(
    IdCategoria INT IDENTITY (1,1) PRIMARY KEY, 
    NombreCat VARCHAR(50) NOT NULL,
    Observacion VARCHAR(50) NULL
);

CREATE TABLE PRODUCTO(
    IdProducto INT IDENTITY (1,1) PRIMARY KEY, 
    NombreProd VARCHAR(100) NOT NULL,
    Unidad VARCHAR(255) NOT NULL,
    Observacion VARCHAR(50) NULL,
    Precio INT NOT NULL,
    IdCategoria INT NOT NULL,
    StockDisponible INT NOT NULL DEFAULT 0,
    Estado BIT NOT NULL DEFAULT 1,               
    CONSTRAINT FK_Producto_Categoria FOREIGN KEY (IdCategoria) REFERENCES CATEGORIA(IdCategoria)
);

CREATE TABLE REGISTRARJORNADA(
    IdRegistrarJornada INT IDENTITY (1,1) PRIMARY KEY, 
    FechaJor DATETIME DEFAULT GETDATE(),
    CantidadPresupuestada INT NOT NULL,
    CantidadDespachada INT NOT NULL,
    IdProducto INT NOT NULL,
    IdUsuario INT NOT NULL,  
    CONSTRAINT FK_RegistrarJornada_Producto FOREIGN KEY (IdProducto) REFERENCES PRODUCTO(IdProducto),
    CONSTRAINT FK_RegistrarJornada_Usuario FOREIGN KEY (IdUsuario) REFERENCES USUARIO(IdUsuario) 
);

CREATE TABLE TRAZABILIDADAUDITORIA(
    IdTrazabilidadAuditoria INT IDENTITY (1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    Accion VARCHAR(100) NOT NULL,
    FechaHora DATETIME DEFAULT GETDATE(),
    Detalle VARCHAR(255) NOT NULL,
    CONSTRAINT FK_Auditoria_Usuario FOREIGN KEY (IdUsuario) REFERENCES USUARIO(IdUsuario)
);
GO*/

USE EmpaGourmet;
GO

-- 1. ELIMINAR TABLA DUPLICADA (Si ya existía en tu entorno de pruebas)
IF OBJECT_ID('log_Productos', 'U') IS NOT NULL DROP TABLE log_Productos;
GO

-- 2. PROCEDIMIENTOS ALMACENADOS OPTIMIZADOS (Sin auditoría manual insertada a la fuerza)
ALTER PROCEDURE sp_InsertarProducto
    @NombreProd VARCHAR(100),                    
    @Unidad VARCHAR(255),                        
    @Observacion VARCHAR(50),
    @Precio INT,
    @IdCategoria INT,
    @StockDisponible INT,
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Establecemos el contexto para que el Trigger sepa quién hace la acción
        EXEC sp_set_session_context 'IdUsuario', @IdUsuario;

        INSERT INTO PRODUCTO (NombreProd, Unidad, Observacion, Precio, IdCategoria, StockDisponible, Estado)
        VALUES (@NombreProd, @Unidad, @Observacion, @Precio, @IdCategoria, @StockDisponible, 1);
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

ALTER PROCEDURE sp_ActualizarProducto
    @IdProducto INT,
    @NombreProd VARCHAR(100),                    
    @Unidad VARCHAR(255),                        
    @Observacion VARCHAR(50),
    @Precio INT,
    @IdCategoria INT,
    @StockDisponible INT,
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        EXEC sp_set_session_context 'IdUsuario', @IdUsuario;

        UPDATE PRODUCTO 
        SET NombreProd = @NombreProd,
            Unidad = @Unidad,
            Observacion = @Observacion,
            Precio = @Precio,
            IdCategoria = @IdCategoria,
            StockDisponible = @StockDisponible
        WHERE IdProducto = @IdProducto AND Estado = 1;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

ALTER PROCEDURE sp_EliminarProducto
    @IdProducto INT,
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        EXEC sp_set_session_context 'IdUsuario', @IdUsuario;

        -- Eliminación lógica (Cambio de estado)
        UPDATE PRODUCTO 
        SET Estado = 0 
        WHERE IdProducto = @IdProducto;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- 3. TRIGGER UNIFICADO Y CENTRALIZADO EN TRAZABILIDADAUDITORIA
ALTER TRIGGER tr_AuditoriaProductos
ON PRODUCTO 
AFTER INSERT, UPDATE, DELETE 
AS 
BEGIN
    SET NOCOUNT ON;

    -- Obtener el usuario desde el contexto seguro de la sesión
    DECLARE @IdUsuario INT = CAST(SESSION_CONTEXT(N'IdUsuario') AS INT);
    
    -- Si por alguna razón el contexto es nulo (ej. ejecución externa accidental), 
    -- puedes asignar un usuario del sistema o forzar un error para cumplir las políticas de seguridad.
    IF @IdUsuario IS NULL SET @IdUsuario = 1; 

    -- REGLA DE NEGOCIO: VALIDACIÓN DE PRECIO
    IF EXISTS(SELECT 1 FROM inserted WHERE Precio <= 0)
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR('Error la Regla: El precio de un producto debe ser mayor a $0.', 16, 1);
        RETURN;
    END

    -- CASO 1: INSERCIÓN
    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO TRAZABILIDADAUDITORIA (IdUsuario, Accion, Detalle)
        SELECT 
            @IdUsuario, 
            'INSERT', 
            CONCAT('Producto creado - ID: ', IdProducto, ', Nombre: ', NombreProd, ', Stock Inicial: ', StockDisponible)
        FROM inserted;
    END

    -- CASO 2: ACTUALIZACIÓN / ELIMINACIÓN LÓGICA
    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        -- Detectar si fue una eliminación lógica (Estado pasó de 1 a 0)
        IF EXISTS (SELECT 1 FROM inserted i JOIN deleted d ON i.IdProducto = d.IdProducto WHERE i.Estado = 0 AND d.Estado = 1)
        BEGIN
            INSERT INTO TRAZABILIDADAUDITORIA (IdUsuario, Accion, Detalle)
            SELECT 
                @IdUsuario, 
                'DELETE', 
                CONCAT('Producto deshabilitado (Eliminación lógica) - ID: ', IdProducto)
            FROM inserted;
        END
        ELSE
        -- Fue una actualización común y corriente
        BEGIN
            INSERT INTO TRAZABILIDADAUDITORIA (IdUsuario, Accion, Detalle)
            SELECT 
                @IdUsuario, 
                'UPDATE', 
                CONCAT('Producto actualizado - ID: ', IdProducto, ', Nuevo Stock: ', StockDisponible, ', Precio: ', Precio)
            FROM inserted;
        END
    END
    
    -- CASO 3: ELIMINACIÓN FÍSICA (Por si acaso alguien ejecuta un DELETE directo en la tabla)
    IF NOT EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO TRAZABILIDADAUDITORIA (IdUsuario, Accion, Detalle)
        SELECT 
            @IdUsuario, 
            'DELETE_FISICO', 
            CONCAT('Producto eliminado físicamente de la tabla - ID: ', IdProducto, ', Nombre: ', NombreProd)
        FROM deleted;
    END
END;
GO

USE EmpaGourmet;
GO

-- =========================================================================
-- 1. REGISTROS PREVIOS OBLIGATORIOS (Para evitar errores de llave foránea)
-- =========================================================================

-- Insertar un Rol si la tabla está vacía
INSERT INTO ROL (Descripcion) VALUES ('Administrador');

-- Insertar un Usuario (será nuestro Administrador con ID = 1)
INSERT INTO USUARIO (Documento, NombreCompleto, Correo, Contrasena, IdRol, Estado)
VALUES ('12345678-9', 'Enzo Ibarguen', 'enzo@empagourmet.cl', 'Admin2026', 1, 1);

-- Insertar Categorías de Empanadas
INSERT INTO CATEGORIA (NombreCat, Observacion) VALUES ('Empanadas Horneadas', 'Empanadas tradicionales al horno');
INSERT INTO CATEGORIA (NombreCat, Observacion) VALUES ('Empanadas Fritas', 'Empanadas para freír al momento');
INSERT INTO CATEGORIA (NombreCat, Observacion) VALUES ('Línea Cóctel', 'Empanadas de tamaño reducido para eventos');
GO


-- =========================================================================
-- 2. INSERCIÓN DE PRODUCTOS MEDIANTE EL PROCEDIMIENTO ALMACENADO
-- =========================================================================
-- Parámetros del SP: @NombreProd, @Unidad, @Observacion, @Precio, @IdCategoria, @StockDisponible, @IdUsuario

-- Producto 1: Caja Empanada de Pino (Categoría 1: Horneadas, Usuario 1)
EXEC sp_InsertarProducto 
    @NombreProd = 'Caja Empanada de Pino Vacuno x6', 
    @Unidad = 'Caja de 6 unidades', 
    @Observacion = 'Receta tradicional chilena con carne picada', 
    @Precio = 12000, 
    @IdCategoria = 1, 
    @StockDisponible = 20, 
    @IdUsuario = 1;

-- Producto 2: Caja Empanada Pollo Cambray (Categoría 1: Horneadas, Usuario 1)
EXEC sp_InsertarProducto 
    @NombreProd = 'Caja Empanada Pollo Cambray x6', 
    @Unidad = 'Caja de 6 unidades', 
    @Observacion = 'Pollo desmenuzado con finas hierbas', 
    @Precio = 11500, 
    @IdCategoria = 1, 
    @StockDisponible = 15, 
    @IdUsuario = 1;

-- Producto 3: Caja Empanada Queso (Categoría 2: Fritas, Usuario 1)
EXEC sp_InsertarProducto 
    @NombreProd = 'Caja Empanada Queso Manta x6', 
    @Unidad = 'Caja de 6 unidades', 
    @Observacion = 'Queso mantecoso de alta fusión', 
    @Precio = 10000, 
    @IdCategoria = 2, 
    @StockDisponible = 30, 
    @IdUsuario = 1;

-- Producto 4: Caja Empanada Camarón Queso (Categoría 2: Fritas, Usuario 1)
EXEC sp_InsertarProducto 
    @NombreProd = 'Caja Empanada Camarón Queso x6', 
    @Unidad = 'Caja de 6 unidades', 
    @Observacion = 'Camarones seleccionados con queso', 
    @Precio = 14000, 
    @IdCategoria = 2, 
    @StockDisponible = 10, 
    @IdUsuario = 1;

-- Producto 5: Caja Cóctel Surtida (Categoría 3: Línea Cóctel, Usuario 1)
EXEC sp_InsertarProducto 
    @NombreProd = 'Caja Empanadas Cóctel Surtidas x24', 
    @Unidad = 'Caja de 24 unidades', 
    @Observacion = '8 pino, 8 pollo, 8 queso mini', 
    @Precio = 18000, 
    @IdCategoria = 3, 
    @StockDisponible = 25, 
    @IdUsuario = 1;
GO

-- =========================================================================
-- 3. VERIFICACIÓN DEL RESULTADO
-- =========================================================================
-- Comprobar que los productos se guardaron correctamente
SELECT * FROM PRODUCTO;

-- Comprobar que el TRIGGER funcionó y escribió de forma transparente en la tabla oficial de auditoría
SELECT * FROM TRAZABILIDADAUDITORIA;


