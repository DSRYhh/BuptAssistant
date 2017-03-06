using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using BuptAssistant.CampusNetwork;
using Xamarin.Forms;

[assembly: Dependency(typeof(BuptAssistant.UWP.NativeCode.NetworkStatus))]
namespace BuptAssistant.UWP.NativeCode
{
    class NetworkStatus : INetworkStatus
    {
        public CampusNetwork.NetworkStatus GetNetworkStatus()
        {
            bool isInternetConnected = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();

            string networkName;
            BuptAssistant.CampusNetwork.NetworkType type;
            if (profile == null)
            {
                return new CampusNetwork.NetworkStatus() {Name = "nullProfile", Type = NetworkType.Others};
            }
            if (profile.IsWlanConnectionProfile)
            {
                networkName = profile.WlanConnectionProfileDetails.GetConnectedSsid();
                type = NetworkType.Wifi;
            }
            else if (profile.IsWwanConnectionProfile)
            {
                networkName = null;
                type = NetworkType.Mobile;
            }
            else
            {
                networkName = null;
                type = NetworkType.Ethernet;
            }

            return new CampusNetwork.NetworkStatus() {Name = networkName, Type = type};
        }
    }
}
