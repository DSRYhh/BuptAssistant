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
        }

        protected override void OnDisappearing()
        {
            UpdateSettingsData();
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            UpdateSettingsData();
            return base.OnBackButtonPressed();
        }

        private void UpdateSettingsData()
        {
            var ecardPassword = this.EcardPassowrd.Value;
            var ecardPasswordKey = this.EcardPassowrd.Key;

        }
    }
}
