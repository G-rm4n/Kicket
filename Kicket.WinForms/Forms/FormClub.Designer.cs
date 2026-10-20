using System.Drawing;
using System.Windows.Forms;

namespace Kicket.WinForms.Forms
{
    partial class FormClub
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            dataGridClubes = new DataGridView();
            ColumnID = new DataGridViewTextBoxColumn();
            ColumnNombre = new DataGridViewTextBoxColumn();
            ColumnAbreviatura = new DataGridViewTextBoxColumn();
            ColumnDescripcion = new DataGridViewTextBoxColumn();
            labelDatos = new Label();
            labelNombre = new Label();
            labelAbreviatura = new Label();
            labelDescripcion = new Label();
            textBoxNombreClub = new TextBox();
            textBoxAbreviatura = new TextBox();
            textBoxDescripcion = new TextBox();
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
            dataGridClubes.AllowUserToAddRows = false;
            dataGridClubes.AllowUserToDeleteRows = false;
            dataGridClubes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridClubes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridClubes.Columns.AddRange(new DataGridViewColumn[] { ColumnID, ColumnNombre, ColumnAbreviatura, ColumnDescripcion });
            dataGridClubes.Location = new Point(18, 225);
            dataGridClubes.MultiSelect = false;
            dataGridClubes.Name = "dataGridClubes";
            dataGridClubes.ReadOnly = true;
            dataGridClubes.RowHeadersWidth = 51;
            dataGridClubes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridClubes.Size = new Size(540, 290);
            dataGridClubes.TabIndex = 0;
            dataGridClubes.CellClick += dataGridClubes_CellClick;
            // 
            // ColumnID
            // 
            ColumnID.DataPropertyName = "ClubId";
            ColumnID.FillWeight = 45F;
            ColumnID.HeaderText = "ID";
            ColumnID.Name = "ColumnID";
            ColumnID.ReadOnly = true;
            // 
            // ColumnNombre
            // 
            ColumnNombre.DataPropertyName = "Nombre";
            ColumnNombre.FillWeight = 120F;
            ColumnNombre.HeaderText = "Nombre";
            ColumnNombre.Name = "ColumnNombre";
            ColumnNombre.ReadOnly = true;
            // 
            // ColumnAbreviatura
            // 
            ColumnAbreviatura.DataPropertyName = "Abreviatura";
            ColumnAbreviatura.FillWeight = 70F;
            ColumnAbreviatura.HeaderText = "Abreviatura";
            ColumnAbreviatura.Name = "ColumnAbreviatura";
            ColumnAbreviatura.ReadOnly = true;
            // 
            // ColumnDescripcion
            // 
            ColumnDescripcion.DataPropertyName = "Descripcion";
            ColumnDescripcion.FillWeight = 140F;
            ColumnDescripcion.HeaderText = "Descripción";
            ColumnDescripcion.Name = "ColumnDescripcion";
            ColumnDescripcion.ReadOnly = true;
            // 
            // labelDatos
            // 
            labelDatos.AutoSize = true;
            labelDatos.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelDatos.Location = new Point(18, 12);
            labelDatos.Name = "labelDatos";
            labelDatos.Size = new Size(204, 37);
            labelDatos.TabIndex = 1;
            labelDatos.Text = "Datos del Club";
            // 
            // labelNombre
            // 
            labelNombre.AutoSize = true;
            labelNombre.Location = new Point(22, 65);
            labelNombre.Name = "labelNombre";
            labelNombre.Size = new Size(71, 20);
            labelNombre.TabIndex = 2;
            labelNombre.Text = "Nombre:";
            // 
            // textBoxNombreClub
            // 
            textBoxNombreClub.Location = new Point(125, 62);
            textBoxNombreClub.Name = "textBoxNombreClub";
            textBoxNombreClub.Size = new Size(240, 27);
            textBoxNombreClub.TabIndex = 3;
            // 
            // labelAbreviatura
            // 
            labelAbreviatura.AutoSize = true;
            labelAbreviatura.Location = new Point(22, 104);
            labelAbreviatura.Name = "labelAbreviatura";
            labelAbreviatura.Size = new Size(89, 20);
            labelAbreviatura.TabIndex = 4;
            labelAbreviatura.Text = "Abreviatura:";
            // 
            // textBoxAbreviatura
            // 
            textBoxAbreviatura.Location = new Point(125, 101);
            textBoxAbreviatura.MaxLength = 10;
            textBoxAbreviatura.Name = "textBoxAbreviatura";
            textBoxAbreviatura.Size = new Size(120, 27);
            textBoxAbreviatura.TabIndex = 5;
            // 
            // labelDescripcion
            // 
            labelDescripcion.AutoSize = true;
            labelDescripcion.Location = new Point(22, 143);
            labelDescripcion.Name = "labelDescripcion";
            labelDescripcion.Size = new Size(89, 20);
            labelDescripcion.TabIndex = 6;
            labelDescripcion.Text = "Descripción:";
            // 
            // textBoxDescripcion
            // 
            textBoxDescripcion.Location = new Point(125, 140);
            textBoxDescripcion.Name = "textBoxDescripcion";
            textBoxDescripcion.Size = new Size(240, 27);
            textBoxDescripcion.TabIndex = 7;
            // 
            // buttonGuardar
            // 
            buttonGuardar.Location = new Point(400, 55);
            buttonGuardar.Name = "buttonGuardar";
            buttonGuardar.Size = new Size(140, 32);
            buttonGuardar.TabIndex = 8;
            buttonGuardar.Text = "Guardar";
            buttonGuardar.UseVisualStyleBackColor = true;
            buttonGuardar.Click += btnGuardar_Click;
            // 
            // buttonModificar
            // 
            buttonModificar.Location = new Point(400, 95);
            buttonModificar.Name = "buttonModificar";
            buttonModificar.Size = new Size(140, 32);
            buttonModificar.TabIndex = 9;
            buttonModificar.Text = "Modificar";
            buttonModificar.UseVisualStyleBackColor = true;
            buttonModificar.Click += buttonModificar_Click;
            // 
            // buttonEliminar
            // 
            buttonEliminar.Location = new Point(400, 135);
            buttonEliminar.Name = "buttonEliminar";
            buttonEliminar.Size = new Size(140, 32);
            buttonEliminar.TabIndex = 10;
            buttonEliminar.Text = "Eliminar";
            buttonEliminar.UseVisualStyleBackColor = true;
            buttonEliminar.Click += buttonEliminar_Click;
            // 
            // buttonLimpiar
            // 
            buttonLimpiar.Location = new Point(400, 175);
            buttonLimpiar.Name = "buttonLimpiar";
            buttonLimpiar.Size = new Size(140, 32);
            buttonLimpiar.TabIndex = 11;
            buttonLimpiar.Text = "Limpiar";
            buttonLimpiar.UseVisualStyleBackColor = true;
            buttonLimpiar.Click += btnLimpiar_Click;
            // 
            // labelListado
            // 
            labelListado.AutoSize = true;
            labelListado.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelListado.Location = new Point(18, 195);
            labelListado.Name = "labelListado";
            labelListado.Size = new Size(155, 23);
            labelListado.TabIndex = 12;
            labelListado.Text = "Listado de Clubes:";
            // 
            // FormClub
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(580, 535);
            Controls.Add(labelListado);
            Controls.Add(buttonLimpiar);
            Controls.Add(buttonEliminar);
            Controls.Add(buttonModificar);
            Controls.Add(buttonGuardar);
            Controls.Add(textBoxDescripcion);
            Controls.Add(labelDescripcion);
            Controls.Add(textBoxAbreviatura);
            Controls.Add(textBoxNombreClub);
            Controls.Add(labelAbreviatura);
            Controls.Add(labelNombre);
            Controls.Add(labelDatos);
            Controls.Add(dataGridClubes);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormClub";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Administración de Clubes";
            Load += FormClub_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridClubes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridClubes = null!;
        private DataGridViewTextBoxColumn ColumnID = null!;
        private DataGridViewTextBoxColumn ColumnNombre = null!;
        private DataGridViewTextBoxColumn ColumnAbreviatura = null!;
        private DataGridViewTextBoxColumn ColumnDescripcion = null!;
        private Label labelDatos = null!;
        private Label labelNombre = null!;
        private Label labelAbreviatura = null!;
        private Label labelDescripcion = null!;
        private TextBox textBoxNombreClub = null!;
        private TextBox textBoxAbreviatura = null!;
        private TextBox textBoxDescripcion = null!;
        private Button buttonGuardar = null!;
        private Button buttonModificar = null!;
        private Button buttonEliminar = null!;
        private Button buttonLimpiar = null!;
        private Label labelListado = null!;
    }
}
