using PM.Entities;
using PM.DAL;
using System;

namespace PM.BLL
{
    public class UsuarioNegocio
    {
        private UsuarioRepository _usuarioRepository = new UsuarioRepository();

        public bool IniciarSesion(string correo, string contrasena)
        {
            // Validaciones básicas de formato antes de ir a la BD
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasena))
                throw new Exception("El correo y la contraseña son obligatorios.");

            USUARIO? usuarioValido = _usuarioRepository.ValidarLogin(correo, contrasena);

            if (usuarioValido != null)
            {
                // Si el usuario existe, lo guardamos en nuestra sesión global del sistema
                SesionActiva.IdUsuario = usuarioValido.IdUsuario;
                SesionActiva.NombreCompleto = usuarioValido.NombreCompleto;
                SesionActiva.IdRol = usuarioValido.IdRol;
                return true;
            }

            return false; // Credenciales incorrectas
        }
    }
}

