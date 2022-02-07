using System;
using System.IO;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using Map = Xamarin.Forms.Maps.Map;
using GPS_Notes.Models;

namespace GPS_Notes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        string _base = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);


        public MapPage()
        {
            Position position = new Position(0, 0);
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(0.444));
            
            Map map = new Map
            {
                MapType = MapType.Satellite
            };

            Content = map;

            InitializeComponent();
            centerAtUser();
        }

        protected override void OnAppearing()
        {
            Mapka.Pins.Clear();

            base.OnAppearing();
            var notes = new List<Note>();
            var files = Directory.EnumerateFiles(_base, "*.txt");
            foreach (var filename in files)
            {
                Console.WriteLine(filename);
                var text = File.ReadAllText(filename).Split(';')[0];
                double lat = Convert.ToDouble(File.ReadAllText(filename).Split(';')[1]);
                double lon = Convert.ToDouble(File.ReadAllText(filename).Split(';')[2]);

                var pin = new CustomPin()
                {
                    Type = PinType.Place,
                    Position = new Position(lat, lon),
                    Label = text.Substring(0, 10) + "...",
                    Filename = filename
                };
                pin.MarkerClicked += Map_PinClicked;
                Mapka.Pins.Add(pin);

                


            }

            centerAtUser();
        }


        async void DodajClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(NewNote));
        }
        async void Map_PinClicked(object sender, PinClickedEventArgs e)
        {

            var pin = sender as CustomPin;
            try
            {
                Console.WriteLine(pin.Filename);
                await Shell.Current.GoToAsync($"{nameof(NewNote)}?{nameof(NewNote.ItemId)}={pin.Filename}");
            }
            catch (Exception)
            {
                Console.WriteLine("Nie udało się załadować notatki");
            }
        }

        async void centerAtUser()
        {
            var location = await Geolocation.GetLocationAsync();

            var lat = location.Latitude;
            var lon = location.Longitude;

            Position position = new Position(lat, lon);
            MapSpan centered = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(0.444));

            Mapka.MoveToRegion(centered);
        }


    }
}