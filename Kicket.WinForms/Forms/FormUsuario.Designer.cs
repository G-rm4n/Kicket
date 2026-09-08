namespace Kicket.WinForms.Forms
{
    partial class FormUsuario
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
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            textBoxNombre = new TextBox();
            textBoxApellido = new TextBox();
            textBoxEmail = new TextBox();
            textBoxPassword = new TextBox();
            dataGridViewUsuarios = new DataGridView();
            ColumnId = new DataGridViewTextBoxColumn();
            ColumnNombre = new DataGridViewTextBoxColumn();
            ColumnApellido = new DataGridViewTextBoxColumn();
            ColumnEmail = new DataGridViewTextBoxColumn();
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
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(133, 12);
            label1.Name = "label1";
            label1.Size = new Size(297, 41);
            label1.TabIndex = 0;
            label1.Text = "Gestion de Usuarios";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(18, 65);
            label2.Name = "label2";
            label2.Size = new Size(64, 20);
            label2.TabIndex = 1;
            label2.Text = "Nombre";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(18, 159);
            label4.Name = "label4";
            label4.Size = new Size(46, 20);
            label4.TabIndex = 3;
            label4.Text = "Email";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(18, 112);
            label5.Name = "label5";
            label5.Size = new Size(66, 20);
            label5.TabIndex = 4;
            label5.Text = "Apellido";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(18, 205);
            label6.Name = "label6";
            label6.Size = new Size(83, 20);
            label6.TabIndex = 13;
            label6.Text = "Contraseña";
            // 
            // textBoxNombre
            // 
            textBoxNombre.Location = new Point(159, 63);
            textBoxNombre.Margin = new Padding(3, 4, 3, 4);
            textBoxNombre.Name = "textBoxNombre";
            textBoxNombre.Size = new Size(114, 27);
            textBoxNombre.TabIndex = 5;
            // 
            // textBoxApellido
            // 
            textBoxApellido.Location = new Point(159, 108);
            textBoxApellido.Margin = new Padding(3, 4, 3, 4);
            textBoxApellido.Name = "textBoxApellido";
            textBoxApellido.Size = new Size(114, 27);
            textBoxApellido.TabIndex = 6;
            // 
            // textBoxEmail
            // 
            textBoxEmail.Location = new Point(159, 155);
            textBoxEmail.Margin = new Padding(3, 4, 3, 4);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.Size = new Size(114, 27);
            textBoxEmail.TabIndex = 7;
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(159, 201);
            textBoxPassword.Margin = new Padding(3, 4, 3, 4);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(114, 27);
            textBoxPassword.TabIndex = 14;
            textBoxPassword.UseSystemPasswordChar = true;
            // 
            // dataGridViewUsuarios
            // 
            dataGridViewUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewUsuarios.Columns.AddRange(new DataGridViewColumn[] { ColumnId, ColumnNombre, ColumnApellido, ColumnEmail });
            dataGridViewUsuarios.Location = new Point(14, 271);
            dataGridViewUsuarios.Margin = new Padding(3, 4, 3, 4);
            dataGridViewUsuarios.Name = "dataGridViewUsuarios";
            dataGridViewUsuarios.RowHeadersWidth = 51;
            dataGridViewUsuarios.Size = new Size(506, 200);
            dataGridViewUsuarios.TabIndex = 8;
            dataGridViewUsuarios.CellClick += dataGridUsuarios_CellClick;
            // 
            // ColumnId
            // 
            ColumnId.DataPropertyName = "IdUsuario";
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
            // ColumnApellido
            // 
            ColumnApellido.DataPropertyName = "Apellido";
            ColumnApellido.HeaderText = "Apellido";
            ColumnApellido.MinimumWidth = 6;
            ColumnApellido.Name = "ColumnApellido";
            // 
            // ColumnEmail
            // 
            ColumnEmail.DataPropertyName = "Email";
            ColumnEmail.HeaderText = "Email";
            ColumnEmail.MinimumWidth = 6;
            ColumnEmail.Name = "ColumnEmail";
            // 
            // buttonGuardar
            // 
            buttonGuardar.Location = new Point(216, 536);
            buttonGuardar.Margin = new Padding(3, 4, 3, 4);
            buttonGuardar.Name = "buttonGuardar";
            buttonGuardar.Size = new Size(86, 31);
            buttonGuardar.TabIndex = 9;
            buttonGuardar.Text = "Guardar";
            buttonGuardar.UseVisualStyleBackColor = true;
            buttonGuardar.Click += buttonGuardar_Click;
            // 
            // buttonModificar
            // 
            buttonModificar.Location = new Point(308, 536);
            buttonModificar.Margin = new Padding(3, 4, 3, 4);
            buttonModificar.Name = "buttonModificar";
            buttonModificar.Size = new Size(86, 31);
            buttonModificar.TabIndex = 10;
            buttonModificar.Text = "Modificar";
            buttonModificar.UseVisualStyleBackColor = true;
            buttonModificar.Click += btnModificar_Click;
            // 
            // buttonEliminar
            // 
            buttonEliminar.Location = new Point(400, 536);
            buttonEliminar.Margin = new Padding(3, 4, 3, 4);
            buttonEliminar.Name = "buttonEliminar";
            buttonEliminar.Size = new Size(86, 31);
            buttonEliminar.TabIndex = 11;
            buttonEliminar.Text = "Eliminar";
            buttonEliminar.UseVisualStyleBackColor = true;
            buttonEliminar.Click += btnEliminar_Click;
            // 
            // buttonLimpiar
            // 
            buttonLimpiar.Location = new Point(492, 536);
            buttonLimpiar.Margin = new Padding(3, 4, 3, 4);
            buttonLimpiar.Name = "buttonLimpiar";
            buttonLimpiar.Size = new Size(86, 31);
            buttonLimpiar.TabIndex = 12;
            buttonLimpiar.Text = "Limpiar";
            buttonLimpiar.UseVisualStyleBackColor = true;
            buttonLimpiar.Click += buttonLimpiar_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 245);
            label3.Name = "label3";
            label3.Size = new Size(220, 20);
            label3.TabIndex = 15;
            label3.Text = "Listado de Usuarios Registrados";
            // 
            // FormUsuario
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = SystemColors.Control;
            ClientSize = new Size(590, 648);
            Controls.Add(label3);
            Controls.Add(buttonLimpiar);
            Controls.Add(buttonEliminar);
            Controls.Add(buttonModificar);
            Controls.Add(buttonGuardar);
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
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormUsuario";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Administración de Usuarios";
            Shown += FormUsuario_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox textBoxNombre;
        private TextBox textBoxApellido;
        private TextBox textBoxEmail;
        private TextBox textBoxPassword;
        private DataGridView dataGridViewUsuarios;
        private DataGridViewTextBoxColumn ColumnId;
        private DataGridViewTextBoxColumn ColumnNombre;
        private DataGridViewTextBoxColumn ColumnApellido;
        private DataGridViewTextBoxColumn ColumnEmail;
        private Button buttonGuardar;
        private Button buttonModificar;
        private Button buttonEliminar;
        private Button buttonLimpiar;
        private Label label3;
    }
}