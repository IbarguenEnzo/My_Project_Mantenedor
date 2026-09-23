using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PM.Entities
{
    public class USUARIO
    {
        public int IdUsuario { get; set; }
        public string Documento { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }

        public USUARIO() { }

        public USUARIO(int idUsuario,
                       string documento,
                       string nombreCompleto,
                       string correo,
                       string contraseña,
                       int idRol,
                       bool estado,
                       DateTime fechaRegistro)
        
        {   IdUsuario = idUsuario;
            Documento = documento;
            NombreCompleto = nombreCompleto;
            Correo = correo;
            Contraseña = contraseña;
            IdRol = idRol;
            Estado = estado;
            FechaRegistro = fechaRegistro;
        }

        public override string ToString()
        {
            return NombreCompleto;


        }
    }
}
