using PM2E13520.Models;
using PM2E13520.Services;

namespace PM2E13520.Views
{
    [QueryProperty(nameof(Sitio), "Sitio")]
    public partial class MapPage : ContentPage
    {
        private readonly GpsService _gps = new();

        private Site _sitio;
        public Site Sitio
        {
            get => _sitio;
            set
            {
                _sitio = value;
                MostrarMapa();
            }
        }

        public MapPage()
        {
            InitializeComponent();
            VerificarGps();
        }

        private async void VerificarGps()
        {
            bool activo = await _gps.GpsActivo();
            LblGpsAviso.IsVisible = !activo;
        }

        private void MostrarMapa()
        {
            if (_sitio == null) return;

            string html = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css' />
                <script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js'></script>
                <style>
                    body {{ margin: 0; padding: 0; }}
                    #map {{ width: 100vw; height: 100vh; }}
                </style>
            </head>
            <body>
                <div id='map'></div>
                <script>
                    var map = L.map('map').setView([{_sitio.Latitud}, {_sitio.Longitud}], 15);
                    L.tileLayer('https://tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png', {{
                        attribution: '© OpenStreetMap'
                    }}).addTo(map);
                    L.marker([{_sitio.Latitud}, {_sitio.Longitud}])
                        .addTo(map)
                        .bindPopup('{_sitio.Descripcion}')
                        .openPopup();
                </script>
            </body>
            </html>";

            MapaWeb.Source = new HtmlWebViewSource { Html = html };
        }
    }
}