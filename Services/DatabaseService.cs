using SQLite;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using proyecto3.Models;

namespace proyecto3.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;
        private readonly string _dbPath;

        public DatabaseService()
        {
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "transacciones.db3");
        }

        private async Task InitAsync()
        {
            if (_database != null)
                return;

            _database = new SQLiteAsyncConnection(_dbPath);
            await _database.CreateTableAsync<Transaccion>();
        }

        public async Task<List<Transaccion>> GetTransaccionesAsync()
        {
            await InitAsync();
            return await _database!.Table<Transaccion>().OrderByDescending(t => t.Fecha).ToListAsync();
        }

        public async Task<int> SaveTransaccionAsync(Transaccion transaccion)
        {
            await InitAsync();
            if (transaccion.Id != 0)
            {
                return await _database!.UpdateAsync(transaccion);
            }
            else
            {
                return await _database!.InsertAsync(transaccion);
            }
        }

        public async Task<int> DeleteTransaccionAsync(Transaccion transaccion)
        {
            await InitAsync();
            return await _database!.DeleteAsync(transaccion);
        }

        public async Task<int> DeleteAllTransaccionesAsync()
        {
            await InitAsync();
            return await _database!.DeleteAllAsync<Transaccion>();
        }
    }
}
