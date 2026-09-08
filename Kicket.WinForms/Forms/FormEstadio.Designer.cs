namespace Kicket.WinForms.Forms
{
    partial class FormEstadio
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            textBoxNombre = new TextBox();
            textBoxDireccion = new TextBox();
            textBoxCiudad = new TextBox();
            dataGridViewEstadios = new DataGridView();
            ColumnId = new DataGridViewTextBoxColumn();
            ColumnNombre = new DataGridViewTextBoxColumn();
            ColumnDireccion = new DataGridViewTextBoxColumn();
            ColumnCiudad = new DataGridViewTextBoxColumn();
            buttonLimpiar = new Button();
            buttonGuardar = new Button();
            buttonEliminar = new Button();
            buttonModificar = new Button();
            label5 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewEstadios).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 12);
            label1.Name = "label1";
            label1.Size = new Size(126, 20);
            label1.TabIndex = 0;
            label1.Text = "Datos del Estadio";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 56);
            label2.Name = "label2";
            label2.Size = new Size(64, 20);
            label2.TabIndex = 1;
            label2.Text = "Nombre";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 99);
            label3.Name = "label3";
            label3.Size = new Size(72, 20);
            label3.TabIndex = 2;
            label3.Text = "Direccion";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 152);
            label4.Name = "label4";
            label4.Size = new Size(56, 20);
            label4.TabIndex = 3;
            label4.Text = "Ciudad";
            // 
            // textBoxNombre
            // 
            textBoxNombre.Location = new Point(127, 45);
            textBoxNombre.Margin = new Padding(3, 4, 3, 4);
            textBoxNombre.Name = "textBoxNombre";
            textBoxNombre.Size = new Size(114, 27);
            textBoxNombre.TabIndex = 4;
            // 
            // textBoxDireccion
            // 
            textBoxDireccion.Location = new Point(127, 95);
            textBoxDireccion.Margin = new Padding(3, 4, 3, 4);
            textBoxDireccion.Name = "textBoxDireccion";
            textBoxDireccion.Size = new Size(114, 27);
            textBoxDireccion.TabIndex = 5;
            // 
            // textBoxCiudad
            // 
            textBoxCiudad.Location = new Point(127, 148);
            textBoxCiudad.Margin = new Padding(3, 4, 3, 4);
            textBoxCiudad.Name = "textBoxCiudad";
            textBoxCiudad.Size = new Size(114, 27);
            textBoxCiudad.TabIndex = 6;
            // 
            // dataGridViewEstadios
            // 
            dataGridViewEstadios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewEstadios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewEstadios.Columns.AddRange(new DataGridViewColumn[] { ColumnId, ColumnNombre, ColumnDireccion, ColumnCiudad });
            dataGridViewEstadios.Location = new Point(12, 251);
            dataGridViewEstadios.Margin = new Padding(3, 4, 3, 4);
            dataGridViewEstadios.Name = "dataGridViewEstadios";
            dataGridViewEstadios.RowHeadersWidth = 51;
            dataGridViewEstadios.Size = new Size(506, 200);
            dataGridViewEstadios.TabIndex = 7;
            dataGridViewEstadios.CellClick += dataGridEstadios_CellClick;
            // 
            // ColumnId
            // 
            ColumnId.DataPropertyName = "IdEstadio";
            ColumnId.HeaderText = "Id";
            ColumnId.MinimumWidth = 6;
            ColumnId.Name = "ColumnId";
            // 
            // ColumnNombre
            // 
            ColumnNombre.DataPropertyName = "Nombre";
            ColumnNombre.HeaderText = "Nombre";
            ColumnNombre.MinimumWidth = 6;
            ColumnNombre.Name = "ColumnNombre";
            // 
            // ColumnDireccion
            // 
            ColumnDireccion.DataPropertyName = "Direccion";
            ColumnDireccion.HeaderText = "Direccion";
            ColumnDireccion.MinimumWidth = 6;
            ColumnDireccion.Name = "ColumnDireccion";
            // 
            // ColumnCiudad
            // 
            ColumnCiudad.DataPropertyName = "Ciudad";
            ColumnCiudad.HeaderText = "Ciudad";
            ColumnCiudad.MinimumWidth = 6;
            ColumnCiudad.Name = "ColumnCiudad";
            // 
            // buttonLimpiar
            // 
            buttonLimpiar.Location = new Point(492, 604);
            buttonLimpiar.Margin = new Padding(3, 4, 3, 4);
            buttonLimpiar.Name = "buttonLimpiar";
            buttonLimpiar.Size = new Size(86, 31);
            buttonLimpiar.TabIndex = 8;
            buttonLimpiar.Text = "Limpiar";
            buttonLimpiar.UseVisualStyleBackColor = true;
            buttonLimpiar.Click += btnLimpiar_Click;
            // 
            // buttonGuardar
            // 
            buttonGuardar.Location = new Point(216, 604);
            buttonGuardar.Margin = new Padding(3, 4, 3, 4);
            buttonGuardar.Name = "buttonGuardar";
            buttonGuardar.Size = new Size(86, 31);
            buttonGuardar.TabIndex = 9;
            buttonGuardar.Text = "Guardar";
            buttonGuardar.UseVisualStyleBackColor = true;
            buttonGuardar.Click += btnGuardar_Click;
            // 
            // buttonEliminar
            // 
            buttonEliminar.Location = new Point(400, 604);
            buttonEliminar.Margin = new Padding(3, 4, 3, 4);
            buttonEliminar.Name = "buttonEliminar";
            buttonEliminar.Size = new Size(86, 31);
            buttonEliminar.TabIndex = 11;
            buttonEliminar.Text = "Eliminar";
            buttonEliminar.UseVisualStyleBackColor = true;
            buttonEliminar.Click += btnEliminar_Click;
            // 
            // buttonModificar
            // 
            buttonModificar.Location = new Point(308, 604);
            buttonModificar.Margin = new Padding(3, 4, 3, 4);
            buttonModificar.Name = "buttonModificar";
            buttonModificar.Size = new Size(86, 31);
            buttonModificar.TabIndex = 12;
            buttonModificar.Text = "Modificar";
            buttonModificar.UseVisualStyleBackColor = true;
            buttonModificar.Click += btnModificar_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 221);
            label5.Name = "label5";
            label5.Size = new Size(219, 20);
            label5.TabIndex = 13;
            label5.Text = "Listado de Estadios Registrados";
            // 
            // FormEstadio
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(590, 648);
            Controls.Add(label5);
            Controls.Add(buttonModificar);
            Controls.Add(buttonEliminar);
            Controls.Add(buttonGuardar);
            Controls.Add(buttonLimpiar);
            Controls.Add(dataGridViewEstadios);
            Controls.Add(textBoxCiudad);
            Controls.Add(textBoxDireccion);
            Controls.Add(textBoxNombre);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormEstadio";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestion de Estadios";
            Shown += FormEstadio_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewEstadios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox textBoxNombre;
        private TextBox textBoxDireccion;
        private TextBox textBoxCiudad;
        private DataGridView dataGridViewEstadios;
        private DataGridViewTextBoxColumn ColumnId;
        private DataGridViewTextBoxColumn ColumnNombre;
        private DataGridViewTextBoxColumn ColumnDireccion;
        private DataGridViewTextBoxColumn ColumnCiudad;
        private Button buttonLimpiar;
        private Button buttonGuardar;
        private Button buttonEliminar;
        private Button buttonModificar;
        private Label label5;
    }
}