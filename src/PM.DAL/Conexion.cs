using Microsoft.Data.SqlClient;

namespace PM.DAL
{
    public static class Conexion
    {
        
        private static readonly string _connectionString = @"Server=ENZO\SQLEXPRESS;Database=EmpaGourmet;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(_connectionString);
        }
    }
}

