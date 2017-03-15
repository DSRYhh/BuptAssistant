using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcardData;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BuptAssistant.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CampusNetworkSsidCollectionPage : ContentPage
    {
        readonly ObservableCollection<string> _records = new ObservableCollection<string>();

        public CampusNetworkSsidCollectionPage()
        {
            InitializeComponent();

            SsidList.ItemsSource = _records;
            GetSsidList();
        }

        private void GetSsidList()
        {
            if (Application.Current.Properties.ContainsKey("CampusNetwork.networkName"))
            {
                var ssidSet = Application.Current.Properties["CampusNetwork.networkName"] as HashSet<string>;

                if (ssidSet != null)
                {
                    _records.Clear();
                    foreach (var ssidItem in ssidSet)
                    {
                        _records.Add(ssidItem);
                    }
                }
            }
        }

        private void AddNewSsidButton_OnClicked(object sender, EventArgs e)
        {
            NewSsid.IsVisible = true;
        }

        private async void NewSsid_OnCompleted(object sender, EventArgs e)
        {
            string ssid = NewSsid.Text;
            var properties = Application.Current.Properties;
            if (Application.Current.Properties.ContainsKey("CampusNetwork.networkName"))
            {
                HashSet<string> ssidSet = Application.Current.Properties["CampusNetwork.networkName"] as HashSet<string>;
                ssidSet?.Add(ssid);
            }
            else
            {
                Application.Current.Properties.Add("CampusNetwork.networkName", new HashSet<string>() {ssid});
            }

            //TODO HashSet<T> can't save locally as a class, only primitive types are supported.
            await Application.Current.SavePropertiesAsync();
            _records.Add(ssid);
            NewSsid.IsVisible = false;
        }

        private void DeleteItem(object sender, EventArgs e)
        {
            var para = ((MenuItem) sender).CommandParameter as string;
            if (Application.Current.Properties.ContainsKey("CampusNetwork.networkName"))
            {
                var ssidSet = Application.Current.Properties["CampusNetwork.networkName"] as HashSet<string>;
                ssidSet?.Remove(para);
            }
        }
    }
}
