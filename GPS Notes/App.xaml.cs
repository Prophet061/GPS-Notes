using GPS_Notes.Services;
using GPS_Notes.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace GPS_Notes
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            CheckPermissions();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();


        }


        public async void CheckPermissions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if(status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                CheckPermissions();
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
