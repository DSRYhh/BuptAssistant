using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BuptAssistant.CampusNetwork;
using BuptAssistant.Settings;
using BuptAssistant.Toolkit;
using Xamarin.Forms;
using EcardData;
using CampusNetwork;
using Plugin.Connectivity;

namespace BuptAssistant
{
    public partial class MainPage
    {
        private HashSet<string> _needLoginSsid = new HashSet<string>() {"BlackTea"};
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Ecard
            if (Application.Current.Properties.ContainsKey("Ecard.enable"))
            {
                bool isEcardEnable = (bool)Application.Current.Properties["Ecard.enable"];
                if (isEcardEnable)
                {
                    //get balance to show on the button
                    var ecardId = Application.Current.Properties["Ecard.id"] as string;
                    var ecardPassword = Application.Current.Properties["Ecard.password"] as string;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        this.EcardButton.IsEnabled = true;
                        try
                        {
                            await GetBalance(ecardId, ecardPassword);
                        }
                        catch (AuthenticationFailedException)
                        {
                            await CrossPlatformFeatures.Toast(this, strings.Alert, strings.LoginFailed, strings.OK);
                        }
                        catch (System.Net.Http.HttpRequestException)
                        {
                            await CrossPlatformFeatures.Toast(this, strings.Alert, strings.NetworkError, strings.OK);
                        }
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        EcardButton.IsEnabled = false;
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


        private async void SettingButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}