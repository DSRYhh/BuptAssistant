using BuptAssistant.Toolkit;
using SQLite;

namespace BuptAssistant.Settings.Ssid
{
    public class SsidDatabaseItem
    {
        [PrimaryKey]
        public string Name { get; set; }
    }
}
