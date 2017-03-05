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
            //DisplayAlert(strings.Alert, strings.EcardNoLoginHint, strings.Login);
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            string userName = UserNameEntry.Text;
            string password = PasswordEntry.Text;
            Application.Current.Properties.Add("Ecard.id", userName);
            Application.Current.Properties.Add("Ecard.password", password);

            await Navigation.PopAsync();
        }
    }
}
