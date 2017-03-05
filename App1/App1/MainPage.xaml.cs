using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using EcardData;
namespace BuptAssistant
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();


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
