using System;
using System.Windows.Forms;
using PM.BLL;
using PM.Entities;

namespace PM.UI.View
{
    public partial class FrmLogin : Form
    {
        // Instanciamos nuestra capa de negocio del usuario
        private readonly UsuarioNegocio _usuarioNegocio = new UsuarioNegocio();

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                string correo = txtCorreo.Text.Trim();
                string clave = txtContrasena.Text.Trim();

                // Validamos credenciales en la capa BLL
                bool loginExitoso = _usuarioNegocio.IniciarSesion(correo, clave);

                if (loginExitoso)
                {
                    MessageBox.Show($"¡Bienvenido(a) al Sistema EmpaGourmet!\n\nUsuario: {SesionActiva.NombreCompleto}",
                                    "Acceso Concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();

                    // Abrimos el mantenedor principal
                    FrmGestionInventario pantallaPrincipal = new FrmGestionInventario();
                    pantallaPrincipal.FormClosed += (s, args) => this.Close();
                    pantallaPrincipal.Show();
                }
                else
                {
                    MessageBox.Show("Las credenciales ingresadas son incorrectas.",
                                    "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validación de Entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
