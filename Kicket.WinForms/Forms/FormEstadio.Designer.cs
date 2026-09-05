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
            ((System.ComponentModel.ISupportInitialize)dataGridViewEstadios).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(97, 15);
            label1.TabIndex = 0;
            label1.Text = "Datos del Estadio";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 42);
            label2.Name = "label2";
            label2.Size = new Size(51, 15);
            label2.TabIndex = 1;
            label2.Text = "Nombre";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 74);
            label3.Name = "label3";
            label3.Size = new Size(57, 15);
            label3.TabIndex = 2;
            label3.Text = "Direccion";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 114);
            label4.Name = "label4";
            label4.Size = new Size(45, 15);
            label4.TabIndex = 3;
            label4.Text = "Ciudad";
            // 
            // textBoxNombre
            // 
            textBoxNombre.Location = new Point(111, 34);
            textBoxNombre.Name = "textBoxNombre";
            textBoxNombre.Size = new Size(100, 23);
            textBoxNombre.TabIndex = 4;
            // 
            // textBoxDireccion
            // 
            textBoxDireccion.Location = new Point(111, 71);
            textBoxDireccion.Name = "textBoxDireccion";
            textBoxDireccion.Size = new Size(100, 23);
            textBoxDireccion.TabIndex = 5;
            // 
            // textBoxCiudad
            // 
            textBoxCiudad.Location = new Point(111, 111);
            textBoxCiudad.Name = "textBoxCiudad";
            textBoxCiudad.Size = new Size(100, 23);
            textBoxCiudad.TabIndex = 6;
            // 
            // dataGridViewEstadios
            // 
            dataGridViewEstadios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewEstadios.Columns.AddRange(new DataGridViewColumn[] { ColumnId, ColumnNombre, ColumnDireccion, ColumnCiudad });
            dataGridViewEstadios.Location = new Point(12, 153);
            dataGridViewEstadios.Name = "dataGridViewEstadios";
            dataGridViewEstadios.Size = new Size(443, 150);
            dataGridViewEstadios.TabIndex = 7;
            dataGridViewEstadios.CellClick += dataGridEstadios_CellClick;
            // 
            // ColumnId
            // 
            ColumnId.DataPropertyName = "Id";
            ColumnId.HeaderText = "Id";
            ColumnId.Name = "ColumnId";
            // 
            // ColumnNombre
            // 
            ColumnNombre.DataPropertyName = "Nombre";
            ColumnNombre.HeaderText = "Nombre";
            ColumnNombre.Name = "ColumnNombre";
            // 
            // ColumnDireccion
            // 
            ColumnDireccion.DataPropertyName = "Direccion";
            ColumnDireccion.HeaderText = "Direccion";
            ColumnDireccion.Name = "ColumnDireccion";
            // 
            // ColumnCiudad
            // 
            ColumnCiudad.DataPropertyName = "Ciudad";
            ColumnCiudad.HeaderText = "Ciudad";
            ColumnCiudad.Name = "ColumnCiudad";
            // 
            // buttonLimpiar
            // 
            buttonLimpiar.Location = new Point(260, 111);
            buttonLimpiar.Name = "buttonLimpiar";
            buttonLimpiar.Size = new Size(75, 23);
            buttonLimpiar.TabIndex = 8;
            buttonLimpiar.Text = "Limpiar";
            buttonLimpiar.UseVisualStyleBackColor = true;
            buttonLimpiar.Click += btnLimpiar_Click;
            // 
            // buttonGuardar
            // 
            buttonGuardar.Location = new Point(16, 331);
            buttonGuardar.Name = "buttonGuardar";
            buttonGuardar.Size = new Size(75, 23);
            buttonGuardar.TabIndex = 9;
            buttonGuardar.Text = "Guardar";
            buttonGuardar.UseVisualStyleBackColor = true;
            buttonGuardar.Click += btnGuardar_Click;
            // 
            // buttonEliminar
            // 
            buttonEliminar.Location = new Point(241, 331);
            buttonEliminar.Name = "buttonEliminar";
            buttonEliminar.Size = new Size(75, 23);
            buttonEliminar.TabIndex = 11;
            buttonEliminar.Text = "Eliminar";
            buttonEliminar.UseVisualStyleBackColor = true;
            buttonEliminar.Click += btnEliminar_Click;
            // 
            // buttonModificar
            // 
            buttonModificar.Location = new Point(135, 331);
            buttonModificar.Name = "buttonModificar";
            buttonModificar.Size = new Size(75, 23);
            buttonModificar.TabIndex = 12;
            buttonModificar.Text = "Modificar";
            buttonModificar.UseVisualStyleBackColor = true;
            buttonModificar.Click += btnModificar_Click;
            // 
            // FormEstadio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(516, 486);
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
            Name = "FormEstadio";
            Text = "Gestion de Estadios";
            Load += FormEstadio_Load;
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
    }
}