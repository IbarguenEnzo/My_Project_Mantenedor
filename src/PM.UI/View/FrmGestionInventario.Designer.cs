namespace PM.UI.View
{
    partial class FrmGestionInventario
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtNombre = new TextBox();
            txtUnidad = new TextBox();
            txtObservacion = new TextBox();
            txtPrecio = new TextBox();
            txtStock = new TextBox();
            cmbCategoria = new ComboBox();
            dgvProductos = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            btnGuardar = new Button();
            btnModificar = new Button();
            btnEliminar = new Button();
            btnCierreSemanal = new Button();
            lblUsuarioConectado = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            SuspendLayout();
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(22, 155);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(100, 23);
            txtNombre.TabIndex = 0;
            // 
            // txtUnidad
            // 
            txtUnidad.Location = new Point(22, 234);
            txtUnidad.Name = "txtUnidad";
            txtUnidad.Size = new Size(100, 23);
            txtUnidad.TabIndex = 1;
            // 
            // txtObservacion
            // 
            txtObservacion.Location = new Point(22, 304);
            txtObservacion.Name = "txtObservacion";
            txtObservacion.Size = new Size(100, 23);
            txtObservacion.TabIndex = 2;
            // 
            // txtPrecio
            // 
            txtPrecio.Location = new Point(22, 366);
            txtPrecio.Name = "txtPrecio";
            txtPrecio.Size = new Size(100, 23);
            txtPrecio.TabIndex = 3;
            // 
            // txtStock
            // 
            txtStock.Location = new Point(22, 433);
            txtStock.Name = "txtStock";
            txtStock.Size = new Size(100, 23);
            txtStock.TabIndex = 4;
            // 
            // cmbCategoria
            // 
            cmbCategoria.FormattingEnabled = true;
            cmbCategoria.Location = new Point(636, 56);
            cmbCategoria.Name = "cmbCategoria";
            cmbCategoria.Size = new Size(284, 23);
            cmbCategoria.TabIndex = 5;
            // 
            // dgvProductos
            // 
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductos.Location = new Point(310, 110);
            dgvProductos.Name = "dgvProductos";
            dgvProductos.Size = new Size(949, 459);
            dgvProductos.TabIndex = 6;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 137);
            label1.Name = "label1";
            label1.Size = new Size(122, 15);
            label1.TabIndex = 7;
            label1.Text = "Nombre del Producto";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(49, 216);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 8;
            label2.Text = "Unidad";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(33, 286);
            label3.Name = "label3";
            label3.Size = new Size(73, 15);
            label3.TabIndex = 9;
            label3.Text = "Observación";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(49, 348);
            label4.Name = "label4";
            label4.Size = new Size(40, 15);
            label4.TabIndex = 10;
            label4.Text = "Precio";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(36, 415);
            label5.Name = "label5";
            label5.Size = new Size(70, 15);
            label5.TabIndex = 11;
            label5.Text = "Stock Inicial";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(730, 38);
            label6.Name = "label6";
            label6.Size = new Size(92, 15);
            label6.TabIndex = 12;
            label6.Text = "Listar Productos";
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(281, 614);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(116, 23);
            btnGuardar.TabIndex = 13;
            btnGuardar.Text = "Guardar Producto";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click_1;
            // 
            // btnModificar
            // 
            btnModificar.Location = new Point(281, 672);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(116, 23);
            btnModificar.TabIndex = 14;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click_1;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(772, 672);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(124, 23);
            btnEliminar.TabIndex = 15;
            btnEliminar.Text = "Eliminar Producto";
            btnEliminar.UseVisualStyleBackColor = true;
            // 
            // btnCierreSemanal
            // 
            btnCierreSemanal.Location = new Point(1394, 672);
            btnCierreSemanal.Name = "btnCierreSemanal";
            btnCierreSemanal.Size = new Size(75, 23);
            btnCierreSemanal.TabIndex = 16;
            btnCierreSemanal.Text = "Realizar";
            btnCierreSemanal.UseVisualStyleBackColor = true;
            // 
            // lblUsuarioConectado
            // 
            lblUsuarioConectado.AutoSize = true;
            lblUsuarioConectado.Location = new Point(1273, 38);
            lblUsuarioConectado.Name = "lblUsuarioConectado";
            lblUsuarioConectado.Size = new Size(114, 15);
            lblUsuarioConectado.TabIndex = 17;
            lblUsuarioConectado.Text = "Usuario: Cargando...";
            // 
            // FrmGestionInventario
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1501, 707);
            Controls.Add(lblUsuarioConectado);
            Controls.Add(btnCierreSemanal);
            Controls.Add(btnEliminar);
            Controls.Add(btnModificar);
            Controls.Add(btnGuardar);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dgvProductos);
            Controls.Add(cmbCategoria);
            Controls.Add(txtStock);
            Controls.Add(txtPrecio);
            Controls.Add(txtObservacion);
            Controls.Add(txtUnidad);
            Controls.Add(txtNombre);
            Name = "FrmGestionInventario";
            Text = "FrmGestionInventario";
            Load += FrmGestionInventario_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtNombre;
        private TextBox txtUnidad;
        private TextBox txtObservacion;
        private TextBox txtPrecio;
        private TextBox txtStock;
        private ComboBox cmbCategoria;
        private DataGridView dgvProductos;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Button btnGuardar;
        private Button btnModificar;
        private Button btnEliminar;
        private Button btnCierreSemanal;
        private Label lblUsuarioConectado;
    }
}