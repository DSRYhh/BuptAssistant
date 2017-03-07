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
            //TODO change hard code SSID to user settings
            BuptAssistant.Droid.NativeCode.NetworkStatus statusGetter = new NetworkStatus();
            var status = statusGetter.GetNetworkStatus();
            if (status.Type == NetworkType.Wifi || status.Name == "BlackTea")
            {
                //TODO change hard code user name and password to user input
                await CampusNetworkLogin.Login("2014210920", "notlove*");
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
            //TODO Start a service to log in.
        }
    }
}