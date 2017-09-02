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
            if (Application.Current.Properties.ContainsKey("CampusNetwork.enable"))
            {
                this.CampusNetworkEnableSwitcher.On = (bool)Application.Current.Properties["CampusNetwork.enable"];
            }
            else
            {
                this.CampusNetworkEnableSwitcher.On = false;
            }

            if (Application.Current.Properties.ContainsKey("CampusNetwork.id"))
            {
                this.CampusNetworkId.Value = Application.Current.Properties["CampusNetwork.id"] as string;
            }
            if (Application.Current.Properties.ContainsKey("CampusNetwork.password"))
            {
                this.CampusNetworkPassowrd.Value = Application.Current.Properties["CampusNetwork.password"] as string;
            }
            if (Application.Current.Properties.ContainsKey("CampusNetwork.networkName"))
            {
                this.CampusNetworkSsid.Value = Application.Current.Properties["CampusNetwork.networkName"] as string;
            }

            if (Application.Current.Properties.ContainsKey("Dorm.id"))
            {
                this.DormIdEntry.Value = Application.Current.Properties["Dorm.id"] as string;
            }
            if (Application.Current.Properties.ContainsKey("Dorm.enable"))
            {
                this.DormEleEnableSwitcher.On = (bool) Application.Current.Properties["Dorm.enable"];
            }

            if (Application.Current.Properties.ContainsKey("Uniform.id"))
            {
                this.UniformIdEntry.Value = Application.Current.Properties["Uniform.id"] as string;
            }
            if (Application.Current.Properties.ContainsKey("Uniform.password"))
            {
                this.UniformPasswordEntry.Value = Application.Current.Properties["Uniform.password"] as string;
            }
        }

        protected override void OnDisappearing()
        {
            UpdateSettingsData();
            base.OnDisappearing();
        }

        private async void UpdateSettingsData()
        {
            var properties = Application.Current.Properties;

            var ecardEnable = EcardEnableSwitcher.On;

            if (ecardEnable)
            {
                var ecardPassword = EcardPassowrd.Value;
                var ecardPasswordKey = EcardPassowrd.Key;

                //TODO: Bug in Huawei Phone password field is not available in Huawei device.
                //To reproduce it, click set EcardPassowrd.isPassword to True and click it after click Username, then it will lose focus and jump focus to username
                //If disable Huawei safety keyboard, this will be fixed.
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

            if (Application.Current.Properties.ContainsKey("CampusNetwork.enable"))
            {
                Application.Current.Properties["CampusNetwork.enable"] = CampusNetworkEnableSwitcher.On;
            }
            else
            {
                Application.Current.Properties.Add("CampusNetwork.enable", CampusNetworkEnableSwitcher.On);
            }

            if (Application.Current.Properties.ContainsKey("Ecard.enable"))
            {
                Application.Current.Properties["Ecard.enable"] = this.EcardEnableSwitcher.On;
            }
            else
            {
                Application.Current.Properties.Add("Ecard.enable", this.EcardEnableSwitcher.On);
            }

            if (Application.Current.Properties.ContainsKey(CampusNetworkId.Key))
            {
                Application.Current.Properties[CampusNetworkId.Key] = CampusNetworkId.Value;
            }
            else
            {
                Application.Current.Properties.Add(CampusNetworkId.Key, CampusNetworkId.Value);
            }

            if (Application.Current.Properties.ContainsKey(CampusNetworkPassowrd.Key))
            {
                Application.Current.Properties[CampusNetworkPassowrd.Key] = CampusNetworkPassowrd.Value;
            }
            else
            {
                Application.Current.Properties.Add(CampusNetworkPassowrd.Key, CampusNetworkPassowrd.Value);
            }

            AddSettingsItem("CampusNetwork.networkName", CampusNetworkSsid.Value);

            AddSettingsItem("Dorm.enable", DormEleEnableSwitcher.On);

            var dormId = DormIdEntry.Value;
            if (!string.IsNullOrEmpty(dormId))
            {
                AddSettingsItem("Dorm.id", dormId);
            }

            var uniformEnable = UniformLoginSwitcher.On;
            AddSettingsItem("Uniform.enable", uniformEnable);
            
            if (uniformEnable)
            {
                AddSettingsItem("Uniform.id", UniformIdEntry.Value);
                AddSettingsItem("Uniform.password", UniformPasswordEntry.Value);
            }

            await Application.Current.SavePropertiesAsync();
        }

        private static void AddSettingsItem(string key, object value)
        {
            var properties = Application.Current.Properties;

            if (properties.ContainsKey(key))
            {
                properties[key] = value;
            }
            else
            {
                properties.Add(key, value);
            }
        }

        private async void GuideToSsidCollectionPageButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CampusNetworkSsidCollectionPage());
        }

        /// <summary>
        /// Dorm query require uniform login enable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CheckIfUniformLoginEnable(object sender, EventArgs e)
        {
            if (UniformLoginSwitcher.On) return;
            await DisplayAlert(strings.Alert, strings.DormUniformLoginRequire, strings.OK);
            DormEleEnableSwitcher.On = false;
        }
    }
}
