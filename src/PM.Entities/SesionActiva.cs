using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Entities
{
    public static class SesionActiva
    {
        public static int IdUsuario { get; set; }
        public static string NombreCompleto { get; set; } = string.Empty;
        public static int IdRol { get; set; }
    }
}