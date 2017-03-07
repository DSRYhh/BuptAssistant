using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BuptAssistant.Ecard
{
    public partial class EcardMainPage
    {
        public EcardMainPage()
        {
            InitializeComponent();

            bool isEcardEnable = (bool)Application.Current.Properties["Ecard.enable"];

            if (!isEcardEnable)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    bool result = await DisplayAlert(strings.Alert, strings.GuideToInputEcardIDandPasswordMessage, strings.OK, strings.No);
                    if (result)
                    {
                        //TODO guide to login page
                        await Navigation.PushAsync(new Ecard.EcardLoginPage());
                    }
                    else
                    {
                        Application.Current.Properties["Ecard.enable"] = false;
                        await Application.Current.SavePropertiesAsync();
                        await Navigation.PopAsync();
                    }
                });
            }
        }
    }
}
