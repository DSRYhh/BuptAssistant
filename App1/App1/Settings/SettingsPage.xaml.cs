using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BuptAssistant.Settings
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            UpdateSettingsData();
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            UpdateSettingsData();
            return base.OnBackButtonPressed();
        }

        private async void UpdateSettingsData()
        {
            var result = await DisplayAlert(strings.Setting, strings.ConfirmSaveChanges, strings.OK, strings.No);

            if (result)
            {
                var ecardPassword = this.EcardPassowrd.Value;
                var ecardPasswordKey = this.EcardPassowrd.Key;
                if (Application.Current.Properties.ContainsKey(ecardPasswordKey))
                {
                    Application.Current.Properties[ecardPasswordKey] = ecardPassword;
                }
                else
                {
                    Application.Current.Properties.Add(ecardPasswordKey,ecardPassword);
                }
                var ecardId = this.EcardId.Value;
                var ecardIdKey = this.EcardId.Key;
                if (Application.Current.Properties.ContainsKey(ecardIdKey))
                {
                    Application.Current.Properties[ecardIdKey] = ecardId;
                }
                else
                {
                    Application.Current.Properties.Add(ecardIdKey, ecardId);
                }

                await Application.Current.SavePropertiesAsync();
            }
        }
    }
}
