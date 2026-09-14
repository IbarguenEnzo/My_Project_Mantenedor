CREATE DATABASE EmpaGourmet;
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
GO



CREATE PROCEDURE sp_InsertarProducto
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
        INSERT INTO PRODUCTO (NombreProd, Unidad, Observacion, Precio, IdCategoria, StockDisponible, Estado)
        VALUES (@NombreProd, @Unidad, @Observacion, @Precio, @IdCategoria, @StockDisponible, 1);
        
        DECLARE @NuevoId INT = SCOPE_IDENTITY();

        INSERT INTO TRAZABILIDADAUDITORIA (IdUsuario, Accion, Detalle)
        VALUES (@IdUsuario, 'INSERT', CONCAT('Producto creado - ID: ', @NuevoId, ', Nombre: ', @NombreProd));

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE sp_ConsultarProducto
    @IdProducto INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @IdProducto IS NULL OR @IdProducto = 0
        SELECT p.IdProducto, p.NombreProd, p.Unidad, p.Observacion, p.Precio, c.NombreCat, p.IdCategoria, p.StockDisponible
        FROM PRODUCTO p
        INNER JOIN CATEGORIA c ON p.IdCategoria = c.IdCategoria
        WHERE p.Estado = 1;
    ELSE
        SELECT p.IdProducto, p.NombreProd, p.Unidad, p.Observacion, p.Precio, c.NombreCat, p.IdCategoria, p.StockDisponible
        FROM PRODUCTO p
        INNER JOIN CATEGORIA c ON p.IdCategoria = c.IdCategoria
        WHERE p.IdProducto = @IdProducto AND p.Estado = 1;
END;
GO

CREATE PROCEDURE sp_ActualizarProducto
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
        UPDATE PRODUCTO 
        SET NombreProd = @NombreProd,
            Unidad = @Unidad,
            Observacion = @Observacion,
            Precio = @Precio,
            IdCategoria = @IdCategoria,
            StockDisponible = @StockDisponible
        WHERE IdProducto = @IdProducto AND Estado = 1;

        INSERT INTO TRAZABILIDADAUDITORIA (IdUsuario, Accion, Detalle)
        VALUES (@IdUsuario, 'UPDATE', CONCAT('Producto actualizado - ID: ', @IdProducto, ', Stock: ', @StockDisponible));

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE sp_EliminarProducto
    @IdProducto INT,
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        UPDATE PRODUCTO 
        SET Estado = 0 
        WHERE IdProducto = @IdProducto;

        INSERT INTO TRAZABILIDADAUDITORIA (IdUsuario, Accion, Detalle)
        VALUES (@IdUsuario, 'DELETE', CONCAT('Producto deshabilitado (Eliminación lógica) - ID: ', @IdProducto));

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
