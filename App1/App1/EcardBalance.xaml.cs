using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcardData;
using Xamarin.Forms;

namespace App1
{
    public partial class EcardBalance : ContentPage
    {
        private List<string> records = new List<string>();

        public EcardBalance()
        {
            InitializeComponent();
            GetRecords();
        }

        private async void GetRecords()
        {
            DateTime start = new DateTime(2017, 2, 21);
            DateTime end = DateTime.Today;
            EcardSystem ecardSystem = new EcardSystem("2014210920", "221414", start, end);
            await ecardSystem.Login();

            var detail = await ecardSystem.GetDetail();
            foreach (var item in detail)
            {
                records.Add(item.ToString());
            }
            EcardRecords.ItemsSource = records;
        }
    }
}
