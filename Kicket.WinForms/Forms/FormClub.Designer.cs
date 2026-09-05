namespace Kicket.WinForms.Forms
{
    partial class FormClub
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
            dataGridClubes = new DataGridView();
            ColumnID = new DataGridViewTextBoxColumn();
            ColumnNombre = new DataGridViewTextBoxColumn();
            ColumnAbreviatura = new DataGridViewTextBoxColumn();
            labelDatos = new Label();
            labelNombre = new Label();
            labelAbreviatura = new Label();
            textBoxNombreClub = new TextBox();
            textBoxAbreviatura = new TextBox();
            buttonGuardar = new Button();
            buttonModificar = new Button();
            buttonEliminar = new Button();
            buttonLimpiar = new Button();
            labelListado = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridClubes).BeginInit();
            SuspendLayout();
            // 
            // dataGridClubes
            // 
            dataGridClubes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridClubes.Columns.AddRange(new DataGridViewColumn[] { ColumnID, ColumnNombre, ColumnAbreviatura });
            dataGridClubes.Location = new Point(12, 140);
            dataGridClubes.Name = "dataGridClubes";
            dataGridClubes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridClubes.Size = new Size(343, 323);
            dataGridClubes.TabIndex = 0;
            dataGridClubes.CellClick += dataGridClubes_CellClick_1;
            dataGridClubes.CellContentClick += dataGridClubes_CellContentClick;
            // 
            // ColumnID
            // 
            ColumnID.DataPropertyName = "Id";
            ColumnID.HeaderText = "ID";
            ColumnID.Name = "ColumnID";
            // 
            // ColumnNombre
            // 
            ColumnNombre.DataPropertyName = "Nombre";
            ColumnNombre.HeaderText = "Nombre";
            ColumnNombre.Name = "ColumnNombre";
            // 
            // ColumnAbreviatura
            // 
            ColumnAbreviatura.DataPropertyName = "Abreviatura";
            ColumnAbreviatura.HeaderText = "Abreviatura";
            ColumnAbreviatura.Name = "ColumnAbreviatura";
            // 
            // labelDatos
            // 
            labelDatos.AutoSize = true;
            labelDatos.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelDatos.Location = new Point(98, 9);
            labelDatos.Name = "labelDatos";
            labelDatos.Size = new Size(156, 30);
            labelDatos.TabIndex = 1;
            labelDatos.Text = "Datos del Club";
            labelDatos.Click += label1_Click;
            // 
            // labelNombre
            // 
            labelNombre.AutoSize = true;
            labelNombre.Location = new Point(21, 54);
            labelNombre.Name = "labelNombre";
            labelNombre.Size = new Size(57, 15);
            labelNombre.TabIndex = 2;
            labelNombre.Text = "Nombre: ";
            // 
            // labelAbreviatura
            // 
            labelAbreviatura.AutoSize = true;
            labelAbreviatura.Location = new Point(21, 83);
            labelAbreviatura.Name = "labelAbreviatura";
            labelAbreviatura.Size = new Size(71, 15);
            labelAbreviatura.TabIndex = 3;
            labelAbreviatura.Text = "Abreviatura:";
            // 
            // textBoxNombreClub
            // 
            textBoxNombreClub.Location = new Point(98, 46);
            textBoxNombreClub.Name = "textBoxNombreClub";
            textBoxNombreClub.Size = new Size(100, 23);
            textBoxNombreClub.TabIndex = 4;
            // 
            // textBoxAbreviatura
            // 
            textBoxAbreviatura.Location = new Point(98, 75);
            textBoxAbreviatura.Name = "textBoxAbreviatura";
            textBoxAbreviatura.Size = new Size(100, 23);
            textBoxAbreviatura.TabIndex = 5;
            // 
            // buttonGuardar
            // 
            buttonGuardar.Location = new Point(12, 488);
            buttonGuardar.Name = "buttonGuardar";
            buttonGuardar.Size = new Size(75, 23);
            buttonGuardar.TabIndex = 6;
            buttonGuardar.Text = "Guardar";
            buttonGuardar.UseVisualStyleBackColor = true;
            buttonGuardar.Click += btnGuardar_Click;
            // 
            // buttonModificar
            // 
            buttonModificar.Location = new Point(113, 488);
            buttonModificar.Name = "buttonModificar";
            buttonModificar.Size = new Size(75, 23);
            buttonModificar.TabIndex = 7;
            buttonModificar.Text = "Modificar";
            buttonModificar.UseVisualStyleBackColor = true;
            // 
            // buttonEliminar
            // 
            buttonEliminar.Location = new Point(230, 488);
            buttonEliminar.Name = "buttonEliminar";
            buttonEliminar.Size = new Size(75, 23);
            buttonEliminar.TabIndex = 8;
            buttonEliminar.Text = "Eliminar";
            buttonEliminar.UseVisualStyleBackColor = true;
            // 
            // buttonLimpiar
            // 
            buttonLimpiar.Location = new Point(230, 66);
            buttonLimpiar.Name = "buttonLimpiar";
            buttonLimpiar.Size = new Size(75, 23);
            buttonLimpiar.TabIndex = 9;
            buttonLimpiar.Text = "Limpiar";
            buttonLimpiar.UseVisualStyleBackColor = true;
            // 
            // labelListado
            // 
            labelListado.AutoSize = true;
            labelListado.Location = new Point(12, 111);
            labelListado.Name = "labelListado";
            labelListado.Size = new Size(164, 15);
            labelListado.TabIndex = 10;
            labelListado.Text = "Listado de Clubes Registrados";
            // 
            // FormClub
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(377, 637);
            Controls.Add(labelListado);
            Controls.Add(buttonLimpiar);
            Controls.Add(buttonEliminar);
            Controls.Add(buttonModificar);
            Controls.Add(buttonGuardar);
            Controls.Add(textBoxAbreviatura);
            Controls.Add(textBoxNombreClub);
            Controls.Add(labelAbreviatura);
            Controls.Add(labelNombre);
            Controls.Add(labelDatos);
            Controls.Add(dataGridClubes);
            Name = "FormClub";
            Text = "Gestion de Clubes";
            Load += FormClub_Load_1;
            ((System.ComponentModel.ISupportInitialize)dataGridClubes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridClubes;
        private Label labelDatos;
        private Label labelNombre;
        private Label labelAbreviatura;
        private TextBox textBoxNombreClub;
        private TextBox textBoxAbreviatura;
        private Button buttonGuardar;
        private Button buttonModificar;
        private Button buttonEliminar;
        private Button buttonLimpiar;
        private Label labelListado;
        private DataGridViewTextBoxColumn ColumnID;
        private DataGridViewTextBoxColumn ColumnNombre;
        private DataGridViewTextBoxColumn ColumnAbreviatura;
    }
}