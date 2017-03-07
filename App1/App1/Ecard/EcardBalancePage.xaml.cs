using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EcardData;
using Xamarin.Forms;
using System.Linq;

namespace BuptAssistant.Ecard
{
    public partial class EcardBalancePage : ContentPage
    {
        ObservableCollection<TransactionRecord> records = new ObservableCollection<TransactionRecord>();
        private DateTime _startQueryTime;
        private DateTime _endQueryTime;


        public EcardBalancePage()
        {

            InitializeComponent();

            EcardRecordsList.ItemsSource = records;

            StartDatePicker.MaximumDate = DateTime.Today;

            StartDatePicker.Date = DateTime.Today.AddDays(-7);
            EndDatePicker.Date = DateTime.Today;

            _startQueryTime = StartDatePicker.Date;
            _endQueryTime = EndDatePicker.Date;
        }

        [Obsolete]
        private async void GuideToLoginPage()
        {
            await Navigation.PushAsync(new EcardLoginPage());
        }

        private async Task GetRecords(DateTime start, DateTime end, bool isSwiped)
        {
            RefreshIndicator.IsVisible = !isSwiped;

            string id = Application.Current.Properties["Ecard.id"] as string;
            string password = Application.Current.Properties["Ecard.password"] as string;
            EcardSystem ecardSystem = new EcardSystem(id, password, start, end);
            await ecardSystem.Login();

            var detail = await ecardSystem.GetDetail();
            detail.Sort((x, y) => y.OperatingDate.CompareTo(x.OperatingDate));
            records.Clear();
            foreach (var item in detail)
            {
                records.Add(item);
            }

            RefreshIndicator.IsVisible = false;
            EcardRecordsList.IsRefreshing = false;
        }

        private async void EcardRecords_Refreshing(object sender, EventArgs e)
        {
            await GetRecords(_startQueryTime,_endQueryTime,true);
        }

        private async void QueryButton_Clicked(object sender, EventArgs e)
        {
            if (StartDatePicker.Date.CompareTo(EndDatePicker.Date) > 0)
            {
                await DisplayAlert(strings.Alert,strings.StartDateAfterEndDate , strings.OK);
                return;
            }
            _startQueryTime = StartDatePicker.Date;
            _endQueryTime = EndDatePicker.Date;

            await GetRecords(_startQueryTime, _endQueryTime,false);
        }

        private void StartDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            EndDatePicker.MinimumDate = StartDatePicker.Date;
        }
    }
}
