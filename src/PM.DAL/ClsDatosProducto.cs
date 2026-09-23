using System;
using System.Data;
using Microsoft.Data.SqlClient;
using PM.Entities;

namespace PM.DAL
{
    // Cambiamos el nombre a ClsDatosProducto para que calce 100% con tu diagrama de clases
    public class ClsDatosProducto
    {
        // 1. MÉTODO: ELIMINAR (Ya lo tenías listo e impecable)
        public bool EliminarProductoLogico(int idProducto, int idUsuario)
        {
            using (SqlConnection conexion = Conexion.ObtenerConexion())
            {
                using (SqlCommand comando = new SqlCommand("sp_EliminarProducto", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@IdProducto", idProducto);
                    comando.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // 2. MÉTODO: INSERTAR (sp_InsertarProducto)
        public bool InsertarProducto(PRODUCTO producto, int idUsuario)
        {
            using (SqlConnection conexion = Conexion.ObtenerConexion())
            {
                using (SqlCommand comando = new SqlCommand("sp_InsertarProducto", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@NombreProd", producto.NombreProd);
                    comando.Parameters.AddWithValue("@Unidad", producto.Unidad);
                    // Si la observación está vacía en la pantalla, mandamos NULL a la BD de forma segura
                    comando.Parameters.AddWithValue("@Observacion", (object)producto.Observacion ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@Precio", producto.Precio);
                    comando.Parameters.AddWithValue("@IdCategoria", producto.IdCategoria);
                    comando.Parameters.AddWithValue("@StockDisponible", producto.StockDisponible);
                    comando.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // 3. MÉTODO: ACTUALIZAR (sp_ActualizarProducto) -> ¡Agregado para completar el CRUD!
        public bool ActualizarProducto(PRODUCTO producto, int idUsuario)
        {
            using (SqlConnection conexion = Conexion.ObtenerConexion())
            {
                using (SqlCommand comando = new SqlCommand("sp_ActualizarProducto", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@IdProducto", producto.IdProducto);
                    comando.Parameters.AddWithValue("@NombreProd", producto.NombreProd);
                    comando.Parameters.AddWithValue("@Unidad", producto.Unidad);
                    comando.Parameters.AddWithValue("@Observacion", (object)producto.Observacion ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@Precio", producto.Precio);
                    comando.Parameters.AddWithValue("@IdCategoria", producto.IdCategoria);
                    comando.Parameters.AddWithValue("@StockDisponible", producto.StockDisponible);
                    comando.Parameters.AddWithValue("@IdUsuario", idUsuario); // Clave para la trazabilidad de la ISO 27001

                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // 4. MÉTODO: CONSULTAR / LISTAR (sp_ConsultarProducto)
        public DataTable ConsultarProducto()
        {
            DataTable tabla = new DataTable();
            using (SqlConnection conexion = Conexion.ObtenerConexion())
            {
                using (SqlCommand comando = new SqlCommand("sp_ConsultarProducto", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                    {
                        conexion.Open();
                        adaptador.Fill(tabla);
                    }
                }
            }
            return tabla;
        }
    }
}
