﻿using System.Collections;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using BuptAssistant.CampusNetwork;
using CampusNetwork;
using Xamarin.Forms;

namespace BuptAssistant.Droid.NativeCode
{
    [Service]
    public class CampusNetworkService : IntentService
    {
        public override IBinder OnBind(Intent intent)
        {
            throw new System.NotImplementedException();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            return base.OnStartCommand(intent, flags, startId);
        }

        protected override async void OnHandleIntent(Intent intent)
        {
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("CampusNetwork.networkName"))
            {
                System.Collections.Generic.HashSet<string> nameList =
                    Xamarin.Forms.Application.Current.Properties["CampusNetwork.networkName"] as
                        System.Collections.Generic.HashSet<string>;

                if (nameList != null)
                {
                    BuptAssistant.Droid.NativeCode.NetworkStatus statusGetter = new NetworkStatus();
                    var status = statusGetter.GetNetworkStatus();
                    if (status.Type == NetworkType.Wifi || nameList.Contains(status.Name))
                    {
                        if (Xamarin.Forms.Application.Current.Properties.ContainsKey("CampusNetwork.enable"))
                        {
                            bool campusNetworkEnable =
                                (bool)Xamarin.Forms.Application.Current.Properties["CampusNetwork.enable"];
                            if (campusNetworkEnable)
                            {
                                if (Xamarin.Forms.Application.Current.Properties.ContainsKey("CampusNetwork.id") &&
                                    Xamarin.Forms.Application.Current.Properties.ContainsKey("CampusNetwork.password"))
                                {
                                    var campusNetworkId = Xamarin.Forms.Application.Current.Properties["CampusNetwork.id"] as string;
                                    var campusNetworkPassword = Xamarin.Forms.Application.Current
                                        .Properties["CampusNetwork.password"] as string;

                                    await CampusNetworkLogin.Login(campusNetworkId, campusNetworkPassword);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { Android.Net.ConnectivityManager.ConnectivityAction })]
    public class NetworkChangeReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Intent serviceIntent = new Intent(context, typeof(CampusNetworkService));
            context.StartService(serviceIntent);
        }
    }
}