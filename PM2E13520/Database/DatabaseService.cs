using SQLite;
using PM2E13520.Models;

namespace PM2E13520.Database
{
    public class DatabaseService
    {
        private ISQLiteAsyncConnection _connection;

        public async Task Init()
        {
            if (_connection != null)
                return;

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "DBSitios.db3");
            _connection = new SQLiteAsyncConnection(dbPath);

            await _connection.CreateTableAsync<Site>();
        }

        public async Task<int> InsertarSitio(Site sitio)
        {
            await Init();
            return await _connection.InsertAsync(sitio);
        }

        public async Task<List<Site>> ObtenerSitios()
        {
            await Init();
            return await _connection.Table<Site>().ToListAsync();
        }

        public async Task<int> EliminarSitio(Site sitio)
        {
            await Init();
            return await _connection.DeleteAsync(sitio);
        }
    }
}
