using proyecto3.Views;
using Microsoft.Maui.Controls;

namespace proyecto3
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registrar ruta para navegación
            Routing.RegisterRoute(nameof(NuevaTransaccionPage), typeof(NuevaTransaccionPage));
        }
    }
}
