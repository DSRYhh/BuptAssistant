using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuptAssistant.CampusNetwork
{
    class NetworkTookit
    {
        public event System.EventHandler NetworkStatusChanged;

        public static void OnNetworkAvailable()
        {
            
        }

        protected virtual void OnNetworkStatusChanged()
        {
            NetworkStatusChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
