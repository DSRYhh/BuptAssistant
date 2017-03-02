using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using EcardData;
namespace App1
{
    public partial class MainPage
    {
        private int Counter { get; set; }
        public MainPage()
        {
            InitializeComponent();
            Counter = 0;

            GetBalance();
        }

        private async void EcardButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EcardBalance());
        }

        private async Task<double> GetBalance()
        {
            DateTime start = new DateTime(2017, 2, 21);
            DateTime end = DateTime.Today;
            EcardSystem ecardSystem = new EcardSystem("2014210920", "221414", start, end);

            await ecardSystem.Login();
            double balance = await ecardSystem.GetBalance();
            EcardButton.Text = balance.ToString();
            return balance;
        }
    }
}
