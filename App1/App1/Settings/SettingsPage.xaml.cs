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
        }

        protected override void OnDisappearing()
        {
            UpdateSettingsData();
            base.OnDisappearing();
        }

        private async void UpdateSettingsData()
        {
            {
                var ecardEnable = EcardEnableSwitcher.On;

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

                if (Application.Current.Properties.ContainsKey("CampusNetwork.enable"))
                {
                    Application.Current.Properties["CampusNetwork.enable"] = CampusNetworkEnableSwitcher.On;
                }
                else
                {
                    Application.Current.Properties.Add("CampusNetwork.enable",CampusNetworkEnableSwitcher.On);
                }
                
                if (Application.Current.Properties.ContainsKey("Ecard.enable"))
                {
                    Application.Current.Properties["Ecard.enable"] = this.EcardEnableSwitcher.On;
                }
                else
                {
                    Application.Current.Properties.Add("Ecard.enable",this.EcardEnableSwitcher.On);
                }

                if (Application.Current.Properties.ContainsKey(CampusNetworkId.Key))
                {
                    Application.Current.Properties[CampusNetworkId.Key] = CampusNetworkId.Value;
                }
                else
                {
                    Application.Current.Properties.Add(CampusNetworkId.Key,CampusNetworkId.Value);
                }

                if (Application.Current.Properties.ContainsKey(CampusNetworkPassowrd.Key))
                {
                    Application.Current.Properties[CampusNetworkPassowrd.Key] = CampusNetworkPassowrd.Value;
                }
                else
                {
                    Application.Current.Properties.Add(CampusNetworkPassowrd.Key,CampusNetworkPassowrd.Value);
                }

                if (Application.Current.Properties.ContainsKey("CampusNetwork.networkName"))
                {
                    Application.Current.Properties["CampusNetwork.networkName"] = this.CampusNetworkSsid.Value;
                }
                else
                {
                    Application.Current.Properties.Add("CampusNetwork.networkName", CampusNetworkSsid.Value);
                }


                if (Application.Current.Properties.ContainsKey("Dorm.enable"))
                {
                    Application.Current.Properties["Dorm.enable"] = DormEleEnableSwitcher.On;
                }
                else
                {
                    Application.Current.Properties.Add("Dorm.enable",DormEleEnableSwitcher.On);
                }

                string dormId = DormIdEntry.Value;
                if (!string.IsNullOrEmpty(dormId))
                {
                    if (Application.Current.Properties.ContainsKey("Dorm.id"))
                    {
                        Application.Current.Properties["Dorm.id"] = dormId;
                    }
                    else
                    {
                        Application.Current.Properties.Add("Dorm.id", dormId);
                    }
                }

                await Application.Current.SavePropertiesAsync();
            }
        }

        private async void GuideToSsidCollectionPageButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CampusNetworkSsidCollectionPage());
        }
    }
}
