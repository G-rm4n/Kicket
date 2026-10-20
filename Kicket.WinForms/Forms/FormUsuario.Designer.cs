using System.Drawing;
using System.Windows.Forms;

namespace Kicket.WinForms.Forms
{
    partial class FormUsuario
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
            label1 = new Label();
            label2 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            labelRol = new Label();
            textBoxNombre = new TextBox();
            textBoxApellido = new TextBox();
            textBoxEmail = new TextBox();
            textBoxPassword = new TextBox();
            comboBoxRol = new ComboBox();
            dataGridViewUsuarios = new DataGridView();
            ColumnId = new DataGridViewTextBoxColumn();
            ColumnNombre = new DataGridViewTextBoxColumn();
            ColumnApellido = new DataGridViewTextBoxColumn();
            ColumnEmail = new DataGridViewTextBoxColumn();
            ColumnRol = new DataGridViewTextBoxColumn();
            buttonGuardar = new Button();
            buttonModificar = new Button();
            buttonEliminar = new Button();
            buttonLimpiar = new Button();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(18, 12);
            label1.Name = "label1";
            label1.Size = new Size(310, 37);
            label1.TabIndex = 0;
            label1.Text = "Gestión de Usuarios";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(22, 65);
            label2.Name = "label2";
            label2.Size = new Size(67, 20);
            label2.TabIndex = 1;
            label2.Text = "Nombre:";
            // 
            // textBoxNombre
            // 
            textBoxNombre.Location = new Point(120, 62);
            textBoxNombre.Name = "textBoxNombre";
            textBoxNombre.Size = new Size(200, 27);
            textBoxNombre.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(345, 65);
            label4.Name = "label4";
            label4.Size = new Size(68, 20);
            label4.TabIndex = 3;
            label4.Text = "Apellido:";
            // 
            // textBoxApellido
            // 
            textBoxApellido.Location = new Point(430, 62);
            textBoxApellido.Name = "textBoxApellido";
            textBoxApellido.Size = new Size(200, 27);
            textBoxApellido.TabIndex = 4;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(22, 108);
            label5.Name = "label5";
            label5.Size = new Size(48, 20);
            label5.TabIndex = 5;
            label5.Text = "Email:";
            // 
            // textBoxEmail
            // 
            textBoxEmail.Location = new Point(120, 105);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.Size = new Size(200, 27);
            textBoxEmail.TabIndex = 6;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(345, 108);
            label6.Name = "label6";
            label6.Size = new Size(85, 20);
            label6.TabIndex = 7;
            label6.Text = "Contraseña:";
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(430, 105);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.PasswordChar = '●';
            textBoxPassword.Size = new Size(200, 27);
            textBoxPassword.TabIndex = 8;
            // 
            // labelRol
            // 
            labelRol.AutoSize = true;
            labelRol.Location = new Point(22, 151);
            labelRol.Name = "labelRol";
            labelRol.Size = new Size(34, 20);
            labelRol.TabIndex = 9;
            labelRol.Text = "Rol:";
            // 
            // comboBoxRol
            // 
            comboBoxRol.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxRol.FormattingEnabled = true;
            comboBoxRol.Location = new Point(120, 148);
            comboBoxRol.Name = "comboBoxRol";
            comboBoxRol.Size = new Size(200, 28);
            comboBoxRol.TabIndex = 10;
            // 
            // buttonGuardar
            // 
            buttonGuardar.Location = new Point(22, 195);
            buttonGuardar.Name = "buttonGuardar";
            buttonGuardar.Size = new Size(110, 32);
            buttonGuardar.TabIndex = 11;
            buttonGuardar.Text = "Guardar";
            buttonGuardar.UseVisualStyleBackColor = true;
            buttonGuardar.Click += buttonGuardar_Click;
            // 
            // buttonModificar
            // 
            buttonModificar.Location = new Point(142, 195);
            buttonModificar.Name = "buttonModificar";
            buttonModificar.Size = new Size(110, 32);
            buttonModificar.TabIndex = 12;
            buttonModificar.Text = "Modificar";
            buttonModificar.UseVisualStyleBackColor = true;
            buttonModificar.Click += btnModificar_Click;
            // 
            // buttonEliminar
            // 
            buttonEliminar.Location = new Point(262, 195);
            buttonEliminar.Name = "buttonEliminar";
            buttonEliminar.Size = new Size(110, 32);
            buttonEliminar.TabIndex = 13;
            buttonEliminar.Text = "Eliminar";
            buttonEliminar.UseVisualStyleBackColor = true;
            buttonEliminar.Click += btnEliminar_Click;
            // 
            // buttonLimpiar
            // 
            buttonLimpiar.Location = new Point(382, 195);
            buttonLimpiar.Name = "buttonLimpiar";
            buttonLimpiar.Size = new Size(110, 32);
            buttonLimpiar.TabIndex = 14;
            buttonLimpiar.Text = "Limpiar";
            buttonLimpiar.UseVisualStyleBackColor = true;
            buttonLimpiar.Click += buttonLimpiar_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(22, 240);
            label3.Name = "label3";
            label3.Size = new Size(265, 23);
            label3.TabIndex = 15;
            label3.Text = "Listado de Usuarios Registrados:";
            // 
            // dataGridViewUsuarios
            // 
            dataGridViewUsuarios.AllowUserToAddRows = false;
            dataGridViewUsuarios.AllowUserToDeleteRows = false;
            dataGridViewUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewUsuarios.Columns.AddRange(new DataGridViewColumn[]
            {
                ColumnId,
                ColumnNombre,
                ColumnApellido,
                ColumnEmail,
                ColumnRol
            });
            dataGridViewUsuarios.Location = new Point(22, 270);
            dataGridViewUsuarios.MultiSelect = false;
            dataGridViewUsuarios.Name = "dataGridViewUsuarios";
            dataGridViewUsuarios.ReadOnly = true;
            dataGridViewUsuarios.RowHeadersWidth = 51;
            dataGridViewUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsuarios.Size = new Size(610, 275);
            dataGridViewUsuarios.TabIndex = 16;
            dataGridViewUsuarios.CellClick += dataGridViewUsuarios_CellClick;
            // 
            // ColumnId
            // 
            ColumnId.DataPropertyName = "IdUsuario";
            ColumnId.FillWeight = 45F;
            ColumnId.HeaderText = "ID";
            ColumnId.Name = "ColumnId";
            ColumnId.ReadOnly = true;
            // 
            // ColumnNombre
            // 
            ColumnNombre.DataPropertyName = "Nombre";
            ColumnNombre.FillWeight = 100F;
            ColumnNombre.HeaderText = "Nombre";
            ColumnNombre.Name = "ColumnNombre";
            ColumnNombre.ReadOnly = true;
            // 
            // ColumnApellido
            // 
            ColumnApellido.DataPropertyName = "Apellido";
            ColumnApellido.FillWeight = 100F;
            ColumnApellido.HeaderText = "Apellido";
            ColumnApellido.Name = "ColumnApellido";
            ColumnApellido.ReadOnly = true;
            // 
            // ColumnEmail
            // 
            ColumnEmail.DataPropertyName = "Email";
            ColumnEmail.FillWeight = 140F;
            ColumnEmail.HeaderText = "Email";
            ColumnEmail.Name = "ColumnEmail";
            ColumnEmail.ReadOnly = true;
            // 
            // ColumnRol
            // 
            ColumnRol.DataPropertyName = "Rol";
            ColumnRol.FillWeight = 75F;
            ColumnRol.HeaderText = "Rol";
            ColumnRol.Name = "ColumnRol";
            ColumnRol.ReadOnly = true;
            // 
            // FormUsuario
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(655, 565);
            Controls.Add(label3);
            Controls.Add(buttonLimpiar);
            Controls.Add(buttonEliminar);
            Controls.Add(buttonModificar);
            Controls.Add(buttonGuardar);
            Controls.Add(comboBoxRol);
            Controls.Add(labelRol);
            Controls.Add(dataGridViewUsuarios);
            Controls.Add(textBoxPassword);
            Controls.Add(label6);
            Controls.Add(textBoxEmail);
            Controls.Add(textBoxApellido);
            Controls.Add(textBoxNombre);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormUsuario";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Administración de Usuarios";
            Shown += FormUsuario_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1 = null!;
        private Label label2 = null!;
        private Label label4 = null!;
        private Label label5 = null!;
        private Label label6 = null!;
        private Label labelRol = null!;
        private TextBox textBoxNombre = null!;
        private TextBox textBoxApellido = null!;
        private TextBox textBoxEmail = null!;
        private TextBox textBoxPassword = null!;
        private ComboBox comboBoxRol = null!;
        private DataGridView dataGridViewUsuarios = null!;
        private DataGridViewTextBoxColumn ColumnId = null!;
        private DataGridViewTextBoxColumn ColumnNombre = null!;
        private DataGridViewTextBoxColumn ColumnApellido = null!;
        private DataGridViewTextBoxColumn ColumnEmail = null!;
        private DataGridViewTextBoxColumn ColumnRol = null!;
        private Button buttonGuardar = null!;
        private Button buttonModificar = null!;
        private Button buttonEliminar = null!;
        private Button buttonLimpiar = null!;
        private Label label3 = null!;
    }
}
