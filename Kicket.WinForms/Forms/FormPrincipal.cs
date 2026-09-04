using Kicket.ApiClient.Abstracciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kicket.WinForms.Forms
{
    public partial class FormPrincipal : Form
    {
        private readonly ISesionUsuario _sesion;
        public FormPrincipal(ISesionUsuario sesion)
        {
            InitializeComponent();

            _sesion = sesion;
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            if (_sesion.Usuario != null)
            {
            lblBienvenida.Text = $"Bienvenido, {_sesion.Usuario.Nombre} {_sesion.Usuario.Apellido}";
            }
        }

        private void BtnCerrarSesion_Click(object sender, EventArgs e)
        {

        }
    }
}
