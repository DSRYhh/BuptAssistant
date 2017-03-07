using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using BuptAssistant.CampusNetwork;
using Xamarin.Forms;
using EcardData;
using CampusNetwork;
using Plugin.Connectivity;

namespace BuptAssistant
{
    public partial class MainPage
    {
        private HashSet<string> needLoginSsid = new HashSet<string>() {"BlackTea"};
        public MainPage()
        {
            InitializeComponent();

            ////Network
            //CrossConnectivity.Current.ConnectivityTypeChanged += async (sender, e) =>
            //{
            //    //var status = DependencyService.Get<INetworkStatus>().GetNetworkStatus();

            //    //if (status.Type == NetworkType.Wifi || needLoginSsid.Contains(status.Name))
            //    //{
            //    //await CampusNetworkLogin.Login("2014210920", "notlove*");
            //    //}
            //};



            //Ecard
            if (Application.Current.Properties.ContainsKey("Ecard.enable"))
            {
                bool isEcardEnable = (bool) Application.Current.Properties["Ecard.enable"];
                if (isEcardEnable)
                {
                    //get balance to show on the button
                    var ecardId = Application.Current.Properties["Ecard.id"] as string;
                    var ecardPassword = Application.Current.Properties["Ecard.password"] as string;

                    Device.BeginInvokeOnMainThread(async () => {
                        await GetBalance(ecardId, ecardPassword);
                    });
                }
            }
            else
            {
                Application.Current.Properties.Add("Ecard.enable", false);
                Device.BeginInvokeOnMainThread(async () => {
                    await Application.Current.SavePropertiesAsync();
                });
            }
        }


        private async void EcardButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Ecard.EcardMainPage());
        }

        private async Task<double> GetBalance(string ecardId, string ecardPassword)
        {
            DateTime start = DateTime.Today.AddDays(-7);
            DateTime end = DateTime.Today;
            EcardSystem ecardSystem = new EcardSystem(ecardId, ecardPassword, start, end);

            await ecardSystem.Login();
            double balance = await ecardSystem.GetBalance();
            if (balance < 100)
            {
                EcardButton.TextColor = Color.Red;
            }
            Device.BeginInvokeOnMainThread(() => {
                EcardButton.Text = strings.Balance + ":" + balance.ToString();
            });

            return balance;
        }

        
    }
}
