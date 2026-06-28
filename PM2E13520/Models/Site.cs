using SQLite;

namespace PM2E13520.Models
{
    public class Site
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Imagen { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Descripcion { get; set; }
    }
}
