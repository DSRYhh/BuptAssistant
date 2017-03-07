using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BuptAssistant.Ecard
{
    public partial class EcardLoginPage : ContentPage
    {
        public EcardLoginPage()
        {
            InitializeComponent();
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            string userName = UserNameEntry.Text;
            string password = PasswordEntry.Text;
            if (!Application.Current.Properties.ContainsKey("Ecard.id"))
            {
                Application.Current.Properties.Add("Ecard.id", userName);
            }
            else
            {
                Application.Current.Properties["Ecard.id"] = userName;

            }
            if (!Application.Current.Properties.ContainsKey("Ecard.password"))
            {
                Application.Current.Properties.Add("Ecard.password", password);
            }
            else
            {
                Application.Current.Properties["Ecard.password"] = password;
            }
            Application.Current.Properties["Ecard.enable"] = true;
            await Application.Current.SavePropertiesAsync();
            await Navigation.PopAsync();
        }
    }
}
