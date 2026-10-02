namespace Kicket.WinForms.Forms
{
    partial class FormSectorDetalle
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
            labelTitulo = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            textBoxNombre = new TextBox();
            textBoxCapacidadMaxima = new TextBox();
            textBoxPrecioBase = new TextBox();
            buttonGuardar = new Button();
            buttonModificar = new Button();
            buttonEliminar = new Button();
            buttonLimpiar = new Button();
            label4 = new Label();
            dataGridViewSectores = new DataGridView();
            ColumnSectorId = new DataGridViewTextBoxColumn();
            ColumnNombre = new DataGridViewTextBoxColumn();
            ColumnCapacidadMaxima = new DataGridViewTextBoxColumn();
            ColumnPrecioBase = new DataGridViewTextBoxColumn();
            buttonCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSectores).BeginInit();
            SuspendLayout();
            // 
            // labelTitulo
            // 
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTitulo.Location = new Point(14, 12);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(160, 25);
            labelTitulo.TabIndex = 0;
            labelTitulo.Text = "Sectores de: ...";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 56);
            label1.Name = "label1";
            label1.Size = new Size(64, 20);
            label1.TabIndex = 1;
            label1.Text = "Nombre";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 96);
            label2.Name = "label2";
            label2.Size = new Size(110, 20);
            label2.TabIndex = 2;
            label2.Text = "Capacidad Max.";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 136);
            label3.Name = "label3";
            label3.Size = new Size(84, 20);
            label3.TabIndex = 3;
            label3.Text = "Precio Base";
            // 
            // textBoxNombre
            // 
            textBoxNombre.Location = new Point(180, 52);
            textBoxNombre.Margin = new Padding(3, 4, 3, 4);
            textBoxNombre.Name = "textBoxNombre";
            textBoxNombre.Size = new Size(220, 27);
            textBoxNombre.TabIndex = 4;
            // 
            // textBoxCapacidadMaxima
            // 
            textBoxCapacidadMaxima.Location = new Point(180, 92);
            textBoxCapacidadMaxima.Margin = new Padding(3, 4, 3, 4);
            textBoxCapacidadMaxima.Name = "textBoxCapacidadMaxima";
            textBoxCapacidadMaxima.Size = new Size(100, 27);
            textBoxCapacidadMaxima.TabIndex = 5;
            // 
            // textBoxPrecioBase
            // 
            textBoxPrecioBase.Location = new Point(180, 132);
            textBoxPrecioBase.Margin = new Padding(3, 4, 3, 4);
            textBoxPrecioBase.Name = "textBoxPrecioBase";
            textBoxPrecioBase.Size = new Size(100, 27);
            textBoxPrecioBase.TabIndex = 6;
            // 
            // buttonGuardar
            // 
            buttonGuardar.Location = new Point(14, 175);
            buttonGuardar.Margin = new Padding(3, 4, 3, 4);
            buttonGuardar.Name = "buttonGuardar";
            buttonGuardar.Size = new Size(86, 31);
            buttonGuardar.TabIndex = 7;
            buttonGuardar.Text = "Guardar";
            buttonGuardar.UseVisualStyleBackColor = true;
            buttonGuardar.Click += btnGuardar_Click;
            // 
            // buttonModificar
            // 
            buttonModificar.Location = new Point(106, 175);
            buttonModificar.Margin = new Padding(3, 4, 3, 4);
            buttonModificar.Name = "buttonModificar";
            buttonModificar.Size = new Size(86, 31);
            buttonModificar.TabIndex = 8;
            buttonModificar.Text = "Modificar";
            buttonModificar.UseVisualStyleBackColor = true;
            buttonModificar.Click += btnModificar_Click;
            // 
            // buttonEliminar
            // 
            buttonEliminar.Location = new Point(198, 175);
            buttonEliminar.Margin = new Padding(3, 4, 3, 4);
            buttonEliminar.Name = "buttonEliminar";
            buttonEliminar.Size = new Size(86, 31);
            buttonEliminar.TabIndex = 9;
            buttonEliminar.Text = "Eliminar";
            buttonEliminar.UseVisualStyleBackColor = true;
            buttonEliminar.Click += btnEliminar_Click;
            // 
            // buttonLimpiar
            // 
            buttonLimpiar.Location = new Point(290, 175);
            buttonLimpiar.Margin = new Padding(3, 4, 3, 4);
            buttonLimpiar.Name = "buttonLimpiar";
            buttonLimpiar.Size = new Size(86, 31);
            buttonLimpiar.TabIndex = 10;
            buttonLimpiar.Text = "Limpiar";
            buttonLimpiar.UseVisualStyleBackColor = true;
            buttonLimpiar.Click += btnLimpiar_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 220);
            label4.Name = "label4";
            label4.Size = new Size(140, 20);
            label4.TabIndex = 11;
            label4.Text = "Listado de Sectores";
            // 
            // dataGridViewSectores
            // 
            dataGridViewSectores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewSectores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewSectores.Columns.AddRange(new DataGridViewColumn[] { ColumnSectorId, ColumnNombre, ColumnCapacidadMaxima, ColumnPrecioBase });
            dataGridViewSectores.Location = new Point(14, 250);
            dataGridViewSectores.Margin = new Padding(3, 4, 3, 4);
            dataGridViewSectores.Name = "dataGridViewSectores";
            dataGridViewSectores.RowHeadersWidth = 51;
            dataGridViewSectores.Size = new Size(480, 180);
            dataGridViewSectores.TabIndex = 12;
            dataGridViewSectores.CellClick += dataGridSectores_CellClick;
            // 
            // ColumnSectorId
            // 
            ColumnSectorId.DataPropertyName = "SectorId";
            ColumnSectorId.HeaderText = "Id";
            ColumnSectorId.MinimumWidth = 6;
            ColumnSectorId.Name = "ColumnSectorId";
            // 
            // ColumnNombre
            // 
            ColumnNombre.DataPropertyName = "Nombre";
            ColumnNombre.HeaderText = "Nombre";
            ColumnNombre.MinimumWidth = 6;
            ColumnNombre.Name = "ColumnNombre";
            // 
            // ColumnCapacidadMaxima
            // 
            ColumnCapacidadMaxima.DataPropertyName = "CapacidadMaxima";
            ColumnCapacidadMaxima.HeaderText = "Capacidad Max.";
            ColumnCapacidadMaxima.MinimumWidth = 6;
            ColumnCapacidadMaxima.Name = "ColumnCapacidadMaxima";
            // 
            // ColumnPrecioBase
            // 
            ColumnPrecioBase.DataPropertyName = "PrecioBase";
            ColumnPrecioBase.HeaderText = "Precio Base";
            ColumnPrecioBase.MinimumWidth = 6;
            ColumnPrecioBase.Name = "ColumnPrecioBase";
            // 
            // buttonCerrar
            // 
            buttonCerrar.Location = new Point(408, 175);
            buttonCerrar.Margin = new Padding(3, 4, 3, 4);
            buttonCerrar.Name = "buttonCerrar";
            buttonCerrar.Size = new Size(86, 31);
            buttonCerrar.TabIndex = 13;
            buttonCerrar.Text = "Cerrar";
            buttonCerrar.UseVisualStyleBackColor = true;
            buttonCerrar.Click += buttonCerrar_Click;
            // 
            // FormSectorDetalle
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(520, 450);
            Controls.Add(buttonCerrar);
            Controls.Add(dataGridViewSectores);
            Controls.Add(label4);
            Controls.Add(buttonLimpiar);
            Controls.Add(buttonEliminar);
            Controls.Add(buttonModificar);
            Controls.Add(buttonGuardar);
            Controls.Add(textBoxPrecioBase);
            Controls.Add(textBoxCapacidadMaxima);
            Controls.Add(textBoxNombre);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(labelTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSectorDetalle";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Sectores del Estadio";
            ((System.ComponentModel.ISupportInitialize)dataGridViewSectores).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTitulo;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBoxNombre;
        private TextBox textBoxCapacidadMaxima;
        private TextBox textBoxPrecioBase;
        private Button buttonGuardar;
        private Button buttonModificar;
        private Button buttonEliminar;
        private Button buttonLimpiar;
        private Label label4;
        private DataGridView dataGridViewSectores;
        private DataGridViewTextBoxColumn ColumnSectorId;
        private DataGridViewTextBoxColumn ColumnNombre;
        private DataGridViewTextBoxColumn ColumnCapacidadMaxima;
        private DataGridViewTextBoxColumn ColumnPrecioBase;
        private Button buttonCerrar;
    }
}