using System;
using System.Data;
using Microsoft.Data.SqlClient;
using PM.Entities;

namespace PM.DAL
{
    public class UsuarioRepository
    {
        // Añadimos el signo '?' para indicar que este método puede retornar un USUARIO o NULL si el login falla
        public USUARIO? ValidarLogin(string correo, string contrasena)
        {
            using (SqlConnection conexion = Conexion.ObtenerConexion())
            {
                string query = "SELECT IdUsuario, Documento, NombreCompleto, Correo, IdRol, Estado, FechaRegistro " +
                               "FROM USUARIO WHERE Correo = @Correo AND Contrasena = @Contrasena AND Estado = 1";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Correo", correo);
                    comando.Parameters.AddWithValue("@Contrasena", contrasena);

                    conexion.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            
                            return new USUARIO(
                                Convert.ToInt32(reader["IdUsuario"]),
                                reader["Documento"]?.ToString() ?? string.Empty,       
                                reader["NombreCompleto"]?.ToString() ?? string.Empty,  
                                reader["Correo"]?.ToString() ?? string.Empty,          
                                string.Empty, // Contraseña vacía por seguridad
                                Convert.ToInt32(reader["IdRol"]),
                                Convert.ToBoolean(reader["Estado"]),
                                Convert.ToDateTime(reader["FechaRegistro"])
                            );
                        }
                    }
                }
            }
            return null; // Si no lo encuentra, retorna nulo
        }
    }
}
