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

            if (Application.Current.Properties.ContainsKey("Ecard.id"))
            {
                this.EcardId.Value = Application.Current.Properties["Ecard.id"] as string;
            }
            if (Application.Current.Properties.ContainsKey("Ecard.password"))
            {
                this.EcardPassowrd.Value = Application.Current.Properties["Ecard.password"] as string;
            }
            if (Application.Current.Properties.ContainsKey("Ecard.enable"))
            {
                this.EcardEnableSwitcher.On = (bool) Application.Current.Properties["Ecard.enable"];
            }
            else
            {
                this.EcardEnableSwitcher.On = false;
            }
        }

        protected override void OnDisappearing()
        {
            UpdateSettingsData();
            base.OnDisappearing();
        }

        private async void UpdateSettingsData()
        {
            var result = await DisplayAlert(strings.Setting, strings.ConfirmSaveChanges, strings.OK, strings.No);

            if (result)
            {
                
                var ecardEnable = this.EcardEnableSwitcher.On;

                if (ecardEnable)
                {
                    var ecardPassword = this.EcardPassowrd.Value;
                    var ecardPasswordKey = this.EcardPassowrd.Key;

                    //TODO: Bug in Huawei Phone password field is not available in Huawei device.
                    //To reproduce it, click set EcardPassowrd.isPassword to True and click it after click Username, then it will lose focus and jump focus to username
                    //If disable Huawei safety keyboard, this will be ok.
                    var ecardId = this.EcardId.Value;
                    var ecardIdKey = this.EcardId.Key;

                    if (Application.Current.Properties.ContainsKey(ecardPasswordKey))
                    {
                        Application.Current.Properties[ecardPasswordKey] = ecardPassword;
                    }
                    else
                    {
                        Application.Current.Properties.Add(ecardPasswordKey, ecardPassword);
                    }

                    if (Application.Current.Properties.ContainsKey(ecardIdKey))
                    {
                        Application.Current.Properties[ecardIdKey] = ecardId;
                    }
                    else
                    {
                        Application.Current.Properties.Add(ecardIdKey, ecardId);
                    }
                }
                
                if (Application.Current.Properties.ContainsKey("Ecard.enable"))
                {
                    Application.Current.Properties["Ecard.enable"] = this.EcardEnableSwitcher.On;
                }
                else
                {
                    Application.Current.Properties.Add("Ecard.enable",this.EcardEnableSwitcher.On);
                }

                await Application.Current.SavePropertiesAsync();
            }
        }
    }
}
