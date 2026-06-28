namespace PM2E13520.Services
{
    public class CameraService
    {
        public async Task<string> TomarFotoBase64()
        {
            try
            {
                var foto = await MediaPicker.CapturePhotoAsync();
                if (foto == null)
                    return null;

                using var stream = await foto.OpenReadAsync();
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al tomar foto: {ex.Message}");
                return null;
            }
        }
    }
}
