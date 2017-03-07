using System;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using BuptAssistant.Droid.NativeCode;
using BuptAssistant.CampusNetwork;
using Xamarin.Forms;

[assembly: Dependency(typeof(BuptAssistant.Droid.NativeCode.NetworkStatus))]
namespace BuptAssistant.Droid.NativeCode
{
    public class NetworkStatus : INetworkStatus
    {
        public CampusNetwork.NetworkStatus GetNetworkStatus()
        {
            ConnectivityManager manager =
                Xamarin.Forms.Forms.Context.GetSystemService(Context.ConnectivityService) as ConnectivityManager;
            NetworkInfo activeNetwork = manager.ActiveNetworkInfo;
            //TODO deal with null activeNetwork: maybe in airplane mode or something
            //TODO deal with null manager
            var networkType = activeNetwork.Type;

            WifiManager wifiManager = (WifiManager)Forms.Context.GetSystemService(Context.WifiService);
            WifiInfo wifiInfo = wifiManager.ConnectionInfo;
            string ssid = wifiInfo.SSID;

            NetworkType type;
            switch (networkType)
            {
                case ConnectivityType.Bluetooth:
                    type = NetworkType.Bluetooth;
                    break;
                case ConnectivityType.Ethernet:
                    type = NetworkType.Ethernet;
                    break;
                case ConnectivityType.Mobile:
                    type = NetworkType.Mobile;
                    break;
                case ConnectivityType.Vpn:
                    type = NetworkType.Vpn;
                    break;
                case ConnectivityType.Wifi:
                    type = NetworkType.Wifi;
                    break;
                default:
                    type = NetworkType.Others;
                    break;

            }

            return new CampusNetwork.NetworkStatus {Name = ssid, Type = type};
        }
    }
}