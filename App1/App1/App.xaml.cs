using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuptAssistant.Toolkit;
using Xamarin.Forms;

namespace BuptAssistant
{
    public partial class App : Application
    {
        private static SettingsDatabase _settingItemsDatabase;

        public static SettingsDatabase SettingItemsDatabase => _settingItemsDatabase ??
                                                               (_settingItemsDatabase =
                                                                   new SettingsDatabase(
                                                                       DependencyService.Get<IFileHelper>().GetLocalFilePath("SettingSQLite.db3")));

        public App()
        {
            InitializeComponent();
            
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
