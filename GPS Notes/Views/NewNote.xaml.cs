using GPS_Notes.Models;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace GPS_Notes.Views
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]

    public partial class NewNote : ContentPage
    {

        string _base = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);


        public string ItemId
        {
            set
            {
                LoadNote(value);
            }
        }

        public NewNote()
        {
            InitializeComponent();
            BindingContext = new Note();
        }

        void LoadNote(string filename)
        {
            try
            {
                string text = File.ReadAllText(filename).Split(';')[0];
                string lat = File.ReadAllText(filename).Split(';')[1];
                string lon = File.ReadAllText(filename).Split(';')[2];

                Note note = new Note
                {
                    NazwaPliku = filename,
                    Tekst = text,
                    Lat = lat,
                    Lon = lon,
                    Data = File.GetCreationTime(filename)

                };
                BindingContext = note;
            }
            catch (Exception)
            {
                Console.WriteLine("Nie udało się załadować notatki");
            }

        }

        async void ZapiszButtonClicked(object sender, EventArgs e)
        {
            var note = (Note)BindingContext;
            if (string.IsNullOrWhiteSpace(note.NazwaPliku))
            {
                Console.WriteLine("new file");

                try
                {

                    string filename = Path.GetRandomFileName() + ".txt";
                    string path = Path.Combine(_base, filename);

                    var location = await Geolocation.GetLocationAsync();
                    Console.WriteLine("Pobieranie lokalizacji");

                    if (location != null)
                    {

                        string lat = location.Latitude.ToString();
                        string lon = location.Longitude.ToString();
                        Console.WriteLine(lat);
                        Console.WriteLine(lon);

                        File.WriteAllText(path, note.Tekst + ";" + lat + ";" + lon);
                        await Shell.Current.GoToAsync("..");

                    } else
                    {
                        await DisplayAlert("", "Nie udało się uzyskać lokalizacji", "OK");

                    }
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    // Handle not supported on device exception
                    await DisplayAlert("", "Urządzenie nie wspiera geolokalizacji", "OK");


                }
                catch (FeatureNotEnabledException fneEx)
                {
                    // Handle not enabled on device exception
                    await DisplayAlert("", "Geolokalizacja nie jest aktywna", "OK"); 

                }
                catch (PermissionException pEx)
                {
                    // Handle permission exception
                    await DisplayAlert("", "Brak uprawnien do geolokalizacji", "OK");

                }
                catch (Exception ex)
                {
                    // Unable to get location
                    await DisplayAlert("", "ERROR: Jakiś tam błąd", "OK");
                }

            }
            else
            {
                Console.WriteLine("edit file");
                File.WriteAllText(note.NazwaPliku, note.Tekst+";"+note.Lat+";"+note.Lon);
                await Shell.Current.GoToAsync("..");

            }

        }

        async void UsunButtonClicked(object sender, EventArgs e)
        {

            
            var note = (Note)BindingContext;
            if (File.Exists(note.NazwaPliku)) File.Delete(note.NazwaPliku);

            await Shell.Current.GoToAsync("..");
        }

    }
}
