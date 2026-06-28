using PM2E13520.Database;
using PM2E13520.Models;
using PM2E13520.Services;

namespace PM2E13520.Views
{
    public partial class ListPage : ContentPage
    {
        private readonly DatabaseService _db = new();
        private readonly ShareService _share = new();

        public ListPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ListaSitios.ItemsSource = await _db.ObtenerSitios();
        }

        private async void OnVerMapaClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Site sitio)
            {
                await Shell.Current.GoToAsync(nameof(MapPage),
                    new Dictionary<string, object> { { "Sitio", sitio } });
            }
        }

        private async void OnCompartirClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Site sitio)
            {
                await _share.CompartirImagen(sitio.Imagen, sitio.Descripcion);
            }
        }

        private async void OnEliminarClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Site sitio)
            {
                bool confirmar = await DisplayAlert("Eliminar",
                    $"¿Desea eliminar el sitio '{sitio.Descripcion}'?",
                    "Sí", "No");

                if (confirmar)
                {
                    await _db.EliminarSitio(sitio);
                    ListaSitios.ItemsSource = await _db.ObtenerSitios();
                }
            }
        }
    }
}
