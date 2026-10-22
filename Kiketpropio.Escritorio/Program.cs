using Kiketpropio.Escritorio.Formularios;

namespace Kiketpropio.Escritorio
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            using var loginForm = new LoginForm();
            if (loginForm.ShowDialog() != DialogResult.OK) return;

            Application.Run(new PrincipalForm());
        }
    }
}
