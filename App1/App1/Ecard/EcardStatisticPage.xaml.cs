﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuptAssistant.Toolkit;
using EcardData;
using Syncfusion.SfChart.XForms;
using Xamarin.Forms;

namespace BuptAssistant.Ecard
{
    public partial class EcardStatisticPage : ContentPage
    {
        private ObservableCollection<ChartDataPoint> records = new ObservableCollection<ChartDataPoint>();
        private DateTime _startQueryTime;
        private DateTime _endQueryTime;

        public EcardStatisticPage()
        {
            InitializeComponent();
            
            Line.ItemsSource = records;

            StartDatePicker.MaximumDate = DateTime.Today;

            StartDatePicker.Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            EndDatePicker.Date = DateTime.Today;

            _startQueryTime = StartDatePicker.Date;
            _endQueryTime = EndDatePicker.Date;

            DataChart.IsVisible = false;
        }

        private async void QueryButton_Clicked(object sender, EventArgs e)
        {
            if (StartDatePicker.Date.CompareTo(EndDatePicker.Date) > 0)
            {
                await DisplayAlert(strings.Alert, strings.StartDateAfterEndDate, strings.OK);
                return;
            }

            _startQueryTime = StartDatePicker.Date;
            _endQueryTime = EndDatePicker.Date;

            await GetRecords(_startQueryTime, _endQueryTime);
        }

        private async Task GetRecords(DateTime startQueryTime, DateTime endQueryTime)
        {
            //TODO AuthenticationFailedException will not be thrown in Android.
            LoadingIndicator.IsVisible = true;
            this.Promotion.IsVisible = false;

            string id = Application.Current.Properties["Ecard.id"] as string;
            string password = Application.Current.Properties["Ecard.password"] as string;
            try
            {
                EcardSystem ecardSystem = new EcardSystem(id, password, startQueryTime, endQueryTime);
                await ecardSystem.Login();

                var detail = await ecardSystem.GetDetail();

                Dictionary<DateTime, double> dailyRecords = new Dictionary<DateTime, double>();
                for (DateTime date = startQueryTime; date <= endQueryTime; date = date.AddDays(1))
                {
                    dailyRecords.Add(date, 0);
                }
                foreach (var item in detail)
                {
                    double amount = double.Parse(item.Amount);
                    DateTime date = item.OperatingDate.Date;
                    if (dailyRecords.ContainsKey(date))
                    {
                        dailyRecords[date] += amount;
                    }
                    else
                    {
                        dailyRecords.Add(date, amount);
                    }
                }

                records.Clear();
                foreach (var item in dailyRecords)
                {
                    records.Add(new ChartDataPoint(item.Key, item.Value));
                }

                DataChart.IsVisible = true;

            }
            catch (AuthenticationFailedException)
            {
                this.Promotion.IsVisible = true;
                this.Promotion.Text = strings.LoginFailed;
                DataChart.IsVisible = false;
                //await CrossPlatformFeatures.Toast(this, strings.Alert, strings.LoginFailed, strings.OK);
            }
            catch (System.Net.Http.HttpRequestException)
            {
                this.Promotion.IsVisible = true;
                this.Promotion.Text = strings.NetworkError;
                DataChart.IsVisible = false;
                //await CrossPlatformFeatures.Toast(this, strings.Alert, strings.NetworkError, strings.OK);
            }
            catch (TaskCanceledException)
            {
                this.Promotion.IsVisible = true;
                this.Promotion.Text = strings.NetworkTimeout;
                DataChart.IsVisible = false;
            }


            LoadingIndicator.IsVisible = false;
        }

        private void StartDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            EndDatePicker.MinimumDate = StartDatePicker.Date;
        }
    }
}
