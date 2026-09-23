using System;
using System.Data;
using System.Windows.Forms;
using PM.BLL;
using PM.Entities;

namespace PM.UI.View
{
    public partial class FrmGestionInventario : Form
    {

        private readonly ClsNegocioProducto _objNegocio = new ClsNegocioProducto();

        public FrmGestionInventario()
        {
            InitializeComponent();
        }


        private void FrmGestionInventario_Load(object sender, EventArgs e)
        {
            try
            {
                //  CARGA AUTOMÁTICA: Llenamos la tabla apenas abra el mantenedor
                ListarProductos();

                //  SEGURIDAD ISO 27001: Mostrar el nombre del usuario conectado en la sesión activa
                lblUsuarioConectado.Text = $"Usuario actual: {SesionActiva.NombreCompleto}";

                //  CONTROL DE ACCESO REAL BASADO EN ROLES (Activado y Seguro)
                if (SesionActiva.IdRol == 2) // Jefatura de Operaciones
                {

                    btnEliminar.Enabled = false;
                    btnCierreSemanal.Enabled = true;
                }
                else if (SesionActiva.IdRol == 1) // Administrador
                {
                    // El Administrador tiene control total
                    btnEliminar.Enabled = true;
                    btnCierreSemanal.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el módulo: {ex.Message}", "Seguridad del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ListarProductos()
        {
            try
            {

                DataTable dt = _objNegocio.Listar();
                dgvProductos.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al listar catálogo: {ex.Message}", "Error de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //  EVENTO DEL BOTÓN GUARDAR 
        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Creamos el objeto entidad con los datos de los TextBox
                PRODUCTO nuevoProducto = new PRODUCTO
                {
                    NombreProd = txtNombre.Text.Trim(),
                    Unidad = txtUnidad.Text.Trim(),
                    Observacion = txtObservacion.Text.Trim(),
                    Precio = Convert.ToInt32(txtPrecio.Text),
                    IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue),
                    StockDisponible = Convert.ToInt32(txtStock.Text)
                };

                // Enviamos el objeto a la BLL pasando el usuario de la sesión activa para la auditoría
                bool exito = _objNegocio.Registrar(nuevoProducto, SesionActiva.IdUsuario);

                if (exito)
                {
                    MessageBox.Show("¡Producto guardado exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarProductos(); // Refrescamos la tabla automáticamente
                    LimpiarFormulario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



        // EVENTO DEL BOTÓN MODIFICAR 
        private void btnModificar_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Validamos que haya una fila seleccionada en la tabla
                if (dgvProductos.CurrentRow == null) throw new Exception("Seleccione un producto de la tabla para modificar.");

                PRODUCTO productoEditado = new PRODUCTO
                {
                    // Rescatamos el ID de la fila seleccionada en el DataGridView
                    IdProducto = Convert.ToInt32(dgvProductos.CurrentRow.Cells["IdProducto"].Value),
                    NombreProd = txtNombre.Text.Trim(),
                    Unidad = txtUnidad.Text.Trim(),
                    Observacion = txtObservacion.Text.Trim(),
                    Precio = Convert.ToInt32(txtPrecio.Text),
                    IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue),
                    StockDisponible = Convert.ToInt32(txtStock.Text)
                };

                bool exito = _objNegocio.Modificar(productoEditado, SesionActiva.IdUsuario);

                if (exito)
                {
                    MessageBox.Show("¡Producto actualizado exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListarProductos();
                    LimpiarFormulario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar: {ex.Message}", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        
        // EVENTO DEL BOTÓN ELIMINAR 
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductos.CurrentRow == null)
                    throw new Exception("Seleccione un producto del listado para proceder con la deshabilitación.");

                int idProducto = Convert.ToInt32(dgvProductos.CurrentRow.Cells["IdProducto"].Value);
                string nombreProd = dgvProductos.CurrentRow.Cells["NombreProd"].Value.ToString() ?? "Producto";

                DialogResult respuesta = MessageBox.Show(
                    $"¿Está seguro de que desea deshabilitar el producto '{nombreProd}' del catálogo general?\n\n(Esta acción mantendrá los registros históricos bajo la norma ISO 27001).",
                    "Confirmación de Baja Lógica",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (respuesta == DialogResult.Yes)
                {
                    bool exito = _objNegocio.Eliminar(idProducto);

                    if (exito)
                    {
                        // La BLL validará automáticamente si el Rol de la sesión activa tiene permisos (Solo Administrador)
                        MessageBox.Show("El producto ha sido deshabilitado correctamente del sistema.", "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ListarProductos();
                        LimpiarFormulario();
                    }
                }
            }
            catch (Exception ex)
            {   
                // Captura tanto errores de selección como la denegación de permisos de la BLL si intenta borrar un Jefe de Operaciones
                MessageBox.Show(ex.Message, "Control de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        // Método auxiliar para limpiar las casillas de texto (Se mantiene al fondo)
        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtUnidad.Clear();
            txtObservacion.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            txtNombre.Focus();
        }
    }
} 

