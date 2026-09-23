using PM.Entities;
using PM.DAL;
using System;
using System.Data;

namespace PM.BLL
{
    public class ClsNegocioProducto
    {
        private readonly ClsDatosProducto _objDatos = new ClsDatosProducto();

        // 1. MÉTODO: REGISTRAR (Establecido previamente)
        public bool Registrar(PRODUCTO obj, int idUsuario)
        {
            if (string.IsNullOrEmpty(obj.NombreProd))
                throw new Exception("El nombre del producto es obligatorio.");

            if (obj.Precio <= 0)
                throw new Exception("El precio debe ser mayor a $0.");

            return _objDatos.InsertarProducto(obj, idUsuario);
        }

        // 2. MÉTODO: LISTAR (Establecido previamente)
        public DataTable Listar()
        {
            return _objDatos.ConsultarProducto();
        }

        // 3. MÉTODO: MODIFICAR (Establecido previamente)
        public bool Modificar(PRODUCTO obj, int idUsuario)
        {
            if (obj.IdProducto <= 0)
                throw new Exception("ID de producto no válido para actualizar.");

            if (obj.Precio <= 0)
                throw new Exception("El precio debe ser mayor a $0.");

            return _objDatos.ActualizarProducto(obj, idUsuario);
        }

        // 4. MÉTODO: ELIMINAR (Establecido previamente)
        public bool Eliminar(int idProducto)
        {
            if (SesionActiva.IdRol != 1)
            {
                throw new Exception("Seguridad del Sistema: Su nivel de usuario (Jefatura) no tiene autorización para eliminar registros.");
            }

            return _objDatos.EliminarProductoLogico(idProducto, SesionActiva.IdUsuario);
        }

        // =========================================================================
        // 5. ¡NUEVO MÉTODO COMPLETO!: CALCULAR ORDEN DE PRODUCCIÓN (RF05 / CU06)
        // =========================================================================
        public int CalcularOrdenProduccion(int stockRemanente)
        {
            // Validación de seguridad física: el stock remanente no puede ser menor a cero
            if (stockRemanente < 0)
                throw new Exception("Error de consistencia: El stock remanente no puede ser un valor negativo.");

            // Meta de producción semanal acumulada definida por EmpaGourmet
            const int MetaSemanal = 500;

            // Flujo Alternativo 2a: Si el stock remanente supera o iguala la meta, no se requiere fabricar más
            if (stockRemanente >= MetaSemanal)
            {
                return 0;
            }

            // Flujo Principal: Aplicación estricta de la fórmula (Producción = 500 - StockRemanente)
            return MetaSemanal - stockRemanente;
        }
    }
}
