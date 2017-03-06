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
            CrossConnectivity.Current.ConnectivityTypeChanged += async (sender, e) =>
            {
                var status = DependencyService.Get<INetworkStatus>().GetNetworkStatus();

                if (status.Type == NetworkType.Wifi || needLoginSsid.Contains(status.Name))
                {
                    await CampusNetworkLogin.Login("2014210920", "notlove*");
                }
            };

            GetBalance();
        }


        private async void EcardButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Ecard.EcardMainPage());
        }

        private async Task<double> GetBalance()
        {
            DateTime start = DateTime.Today.AddDays(-7);
            DateTime end = DateTime.Today;
            EcardSystem ecardSystem = new EcardSystem("2014210920", "221414", start, end);

            await ecardSystem.Login();
            double balance = await ecardSystem.GetBalance();
            if (balance < 100)
            {
                EcardButton.TextColor = Color.Red;
            }
            EcardButton.Text = strings.Balance + ":" + balance.ToString();
            return balance;
        }

        private void TestButton_OnClicked(object sender, EventArgs e)
        {
            
        }
    }
}
