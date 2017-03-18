using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuptAssistant.Settings.Ssid;
using BuptAssistant.Toolkit;
using Xamarin.Forms;

namespace BuptAssistant
{
    public partial class App : Application
    {
        private static SsidTable _settingItemsDatabase;

        public static SsidTable SettingItemsDatabase => _settingItemsDatabase ??
                                                               (_settingItemsDatabase =
                                                                   new SsidTable(
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
