using PM2E13520.Database;
using PM2E13520.Models;
using PM2E13520.Services;

namespace PM2E13520.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly CameraService _camera = new();
        private readonly GpsService _gps = new();
        private readonly DatabaseService _db = new();

        private string _imagenBase64;

        public HomePage()
        {
            InitializeComponent();
        }

        private async void OnTomarFotoClicked(object sender, EventArgs e)
        {
            // 1. Permiso de cámara
            var statusCamara = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (statusCamara != PermissionStatus.Granted)
                statusCamara = await Permissions.RequestAsync<Permissions.Camera>();

            if (statusCamara != PermissionStatus.Granted)
            {
                await DisplayAlert("Permiso requerido", "Se necesita acceso a la cámara para tomar la foto.", "OK");
                return;
            }

            // 2. Permiso de ubicación
            var statusUbicacion = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (statusUbicacion != PermissionStatus.Granted)
                statusUbicacion = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (statusUbicacion != PermissionStatus.Granted)
            {
                await DisplayAlert("Permiso requerido", "Se necesita acceso a la ubicación para guardar el sitio.", "OK");
                return;
            }

            // 3. GPS activo
            bool gpsActivo = await _gps.GpsActivo();
            if (!gpsActivo)
            {
                await DisplayAlert("GPS inactivo", "Active el GPS antes de tomar la foto.", "OK");
                return;
            }

            // 4. Tomar la foto
            _imagenBase64 = await _camera.TomarFotoBase64();

            if (_imagenBase64 != null)
            {
                byte[] bytes = Convert.FromBase64String(_imagenBase64);
                ImgSitio.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
                ImgSitio.IsVisible = true;
                ImgPlaceholder.IsVisible = false;
                ubicacion();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo tomar la foto.", "OK");
            }
        }

        private async void OnObtenerGpsClicked(object sender, EventArgs e)
        {
        
        }

        private async void ubicacion()
        {
            var ubicacion = await _gps.ObtenerUbicacion();

            if (ubicacion != null)
            {
                EntLatitud.Text = ubicacion.Latitude.ToString();
                EntLongitud.Text = ubicacion.Longitude.ToString();
            }
            else
            {
                await DisplayAlert("GPS inactivo",
                    "No se pudo obtener la ubicación. Verifique que el GPS esté activo.",
                    "OK");
            }
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(EntDescripcion.Text))
            {
                await DisplayAlert("Aviso", "Ingrese una descripción.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(EntLatitud.Text) || string.IsNullOrWhiteSpace(EntLongitud.Text))
            {
                await DisplayAlert("Aviso", "Primero obtenga la ubicación GPS.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(_imagenBase64))
            {
                await DisplayAlert("Aviso", "Tome una foto del sitio.", "OK");
                return;
            }

            var sitio = new Site
            {
                Imagen = _imagenBase64,
                Latitud = double.Parse(EntLatitud.Text),
                Longitud = double.Parse(EntLongitud.Text),
                Descripcion = EntDescripcion.Text
            };

            await _db.InsertarSitio(sitio);
            await DisplayAlert("Éxito", "Sitio guardado correctamente.", "OK");

            await Shell.Current.GoToAsync("//ListPage");
            _imagenBase64 = null;
            ImgSitio.Source = null;
            EntDescripcion.Text = string.Empty;
            EntLatitud.Text = string.Empty;
            EntLongitud.Text = string.Empty;
            
        }
    }
}
