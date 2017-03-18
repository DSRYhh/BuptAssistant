using System.Collections.Generic;
using System.Threading.Tasks;
using BuptAssistant.Toolkit;
using SQLite;

namespace BuptAssistant.Settings.Ssid
{
    public class SsidTable : Toolkit.ISettingsDatabase<SsidDatabaseItem,string>
    {
        private readonly SQLiteAsyncConnection _database;

        public SsidTable(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<SsidDatabaseItem>().Wait();
            
        }

        public Task<List<SsidDatabaseItem>> GetItemsAsync()
        {
            var database = _database;
            var table = database.Table<SsidDatabaseItem>();

            return _database.Table<SsidDatabaseItem>().ToListAsync();
        }

        public Task<SsidDatabaseItem> GetItemAsync(string key)
        {
            return _database.Table<SsidDatabaseItem>().Where(i => i.Name == key).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(SsidDatabaseItem item)
        {
            //if ((await GetItemAsync(item.Name) == null)//check if ssid item already existed
            //{
            //TODO need check if ssid item already existed? Try to add two same ssid.
                return _database.InsertAsync(item);
            //}
        }

        public Task<int> DeleteItemAsync(SsidDatabaseItem item)
        {
            return _database.DeleteAsync(item);
        }
    }
}