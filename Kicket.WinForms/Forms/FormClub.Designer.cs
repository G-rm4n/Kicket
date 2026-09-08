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
            dataGridClubes.Location = new Point(14, 187);
            dataGridClubes.Margin = new Padding(3, 4, 3, 4);
            dataGridClubes.Name = "dataGridClubes";
            dataGridClubes.RowHeadersWidth = 51;
            dataGridClubes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridClubes.Size = new Size(392, 315);
            dataGridClubes.TabIndex = 0;
            dataGridClubes.CellClick += dataGridClubes_CellClick;
            // 
            // ColumnID
            // 
            ColumnID.DataPropertyName = "ClubId";
            ColumnID.HeaderText = "ID";
            ColumnID.MinimumWidth = 6;
            ColumnID.Name = "ColumnID";
            ColumnID.Width = 125;
            // 
            // ColumnNombre
            // 
            ColumnNombre.DataPropertyName = "Nombre";
            ColumnNombre.HeaderText = "Nombre";
            ColumnNombre.MinimumWidth = 6;
            ColumnNombre.Name = "ColumnNombre";
            ColumnNombre.Width = 125;
            // 
            // ColumnAbreviatura
            // 
            ColumnAbreviatura.DataPropertyName = "Abreviatura";
            ColumnAbreviatura.HeaderText = "Abreviatura";
            ColumnAbreviatura.MinimumWidth = 6;
            ColumnAbreviatura.Name = "ColumnAbreviatura";
            ColumnAbreviatura.Width = 125;
            // 
            // labelDatos
            // 
            labelDatos.AutoSize = true;
            labelDatos.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelDatos.Location = new Point(112, 12);
            labelDatos.Name = "labelDatos";
            labelDatos.Size = new Size(204, 37);
            labelDatos.TabIndex = 1;
            labelDatos.Text = "Datos del Club";
            // 
            // labelNombre
            // 
            labelNombre.AutoSize = true;
            labelNombre.Location = new Point(24, 72);
            labelNombre.Name = "labelNombre";
            labelNombre.Size = new Size(71, 20);
            labelNombre.TabIndex = 2;
            labelNombre.Text = "Nombre: ";
            // 
            // labelAbreviatura
            // 
            labelAbreviatura.AutoSize = true;
            labelAbreviatura.Location = new Point(24, 111);
            labelAbreviatura.Name = "labelAbreviatura";
            labelAbreviatura.Size = new Size(89, 20);
            labelAbreviatura.TabIndex = 3;
            labelAbreviatura.Text = "Abreviatura:";
            // 
            // textBoxNombreClub
            // 
            textBoxNombreClub.Location = new Point(112, 61);
            textBoxNombreClub.Margin = new Padding(3, 4, 3, 4);
            textBoxNombreClub.Name = "textBoxNombreClub";
            textBoxNombreClub.Size = new Size(114, 27);
            textBoxNombreClub.TabIndex = 4;
            // 
            // textBoxAbreviatura
            // 
            textBoxAbreviatura.Location = new Point(112, 100);
            textBoxAbreviatura.Margin = new Padding(3, 4, 3, 4);
            textBoxAbreviatura.Name = "textBoxAbreviatura";
            textBoxAbreviatura.Size = new Size(114, 27);
            textBoxAbreviatura.TabIndex = 5;
            // 
            // buttonGuardar
            // 
            buttonGuardar.Location = new Point(216, 604);
            buttonGuardar.Margin = new Padding(3, 4, 3, 4);
            buttonGuardar.Name = "buttonGuardar";
            buttonGuardar.Size = new Size(86, 31);
            buttonGuardar.TabIndex = 6;
            buttonGuardar.Text = "Guardar";
            buttonGuardar.UseVisualStyleBackColor = true;
            buttonGuardar.Click += btnGuardar_Click;
            // 
            // buttonModificar
            // 
            buttonModificar.Location = new Point(308, 604);
            buttonModificar.Margin = new Padding(3, 4, 3, 4);
            buttonModificar.Name = "buttonModificar";
            buttonModificar.Size = new Size(86, 31);
            buttonModificar.TabIndex = 7;
            buttonModificar.Text = "Modificar";
            buttonModificar.UseVisualStyleBackColor = true;
            buttonModificar.Click += buttonModificar_Click;
            // 
            // buttonEliminar
            // 
            buttonEliminar.Location = new Point(400, 604);
            buttonEliminar.Margin = new Padding(3, 4, 3, 4);
            buttonEliminar.Name = "buttonEliminar";
            buttonEliminar.Size = new Size(86, 31);
            buttonEliminar.TabIndex = 8;
            buttonEliminar.Text = "Eliminar";
            buttonEliminar.UseVisualStyleBackColor = true;
            buttonEliminar.Click += buttonEliminar_Click;
            // 
            // buttonLimpiar
            // 
            buttonLimpiar.Location = new Point(492, 604);
            buttonLimpiar.Margin = new Padding(3, 4, 3, 4);
            buttonLimpiar.Name = "buttonLimpiar";
            buttonLimpiar.Size = new Size(86, 31);
            buttonLimpiar.TabIndex = 9;
            buttonLimpiar.Text = "Limpiar";
            buttonLimpiar.UseVisualStyleBackColor = true;
            buttonLimpiar.Click += btnLimpiar_Click;
            // 
            // labelListado
            // 
            labelListado.AutoSize = true;
            labelListado.Location = new Point(14, 148);
            labelListado.Name = "labelListado";
            labelListado.Size = new Size(208, 20);
            labelListado.TabIndex = 10;
            labelListado.Text = "Listado de Clubes Registrados";
            // 
            // FormClub
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(590, 648);
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
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormClub";
            Text = "Gestion de Clubes";
            Shown += FormClub_Load;
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