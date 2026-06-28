namespace PM2E13520.Services
{
    public class GpsService
    {
        public async Task<Location> ObtenerUbicacion()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                    return null;

                var ubicacion = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(10)
                });

                return ubicacion;
            }
            catch (FeatureNotEnabledException)
            {
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error GPS: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> GpsActivo()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                    return false;

                var ubicacion = await Geolocation.GetLastKnownLocationAsync();
                return ubicacion != null;
            }
            catch
            {
                return false;
            }
        }
    }
}