using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace BuptAssistant.Toolkit
{
    public interface ISettingsDatabase<TItem, in TKey>
    {
        Task<List<TItem>> GetItemsAsync();

        Task<TItem> GetItemAsync(TKey key);

        Task<int> SaveItemAsync(TItem item);

        Task<int> DeleteItemAsync(TItem item);
    }
}