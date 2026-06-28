namespace PM2E13520.Services
{
    public class ShareService
    {
        public async Task CompartirImagen(string base64, string descripcion)
        {
            try
            {
                if (string.IsNullOrEmpty(base64))
                {
                    await Shell.Current.DisplayAlert("Aviso", "Este sitio no tiene imagen para compartir.", "OK");
                    return;
                }

                byte[] bytes = Convert.FromBase64String(base64);
                string ruta = Path.Combine(FileSystem.CacheDirectory, "sitio_compartido.jpg");
                await File.WriteAllBytesAsync(ruta, bytes);

                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = descripcion ?? "Sitio compartido",
                    File = new ShareFile(ruta)
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al compartir: {ex.Message}");
            }
        }
    }
}
