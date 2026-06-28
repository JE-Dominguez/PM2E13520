using PM2E13520.Views;

namespace PM2E13520
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registrar la ruta de MapPage para navegación desde ListPage
            Routing.RegisterRoute(nameof(MapPage), typeof(MapPage));
        }
    }
}
