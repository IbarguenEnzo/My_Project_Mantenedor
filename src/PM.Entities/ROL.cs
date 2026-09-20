using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Entities
{
    public class ROL
    {
        public int IdRol { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }

        public ROL() { }

        public ROL(int rol, string descripcion, DateTime fechaRegistro)
        {
            IdRol = rol;
            Descripcion = descripcion;
            FechaRegistro = fechaRegistro;
        }


    }
}
