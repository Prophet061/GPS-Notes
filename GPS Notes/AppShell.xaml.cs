using GPS_Notes.ViewModels;
using GPS_Notes.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GPS_Notes
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewNote), typeof(NewNote));
            Routing.RegisterRoute(nameof(MapPage), typeof(MapPage));
        }

    }
}
