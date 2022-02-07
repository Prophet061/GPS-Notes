using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using GPS_Notes.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GPS_Notes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class NotesPage : ContentPage
    {
        string _base = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);


        protected override void OnAppearing()
        {
            base.OnAppearing();
            var notes = new List<Note>();
            var files = Directory.EnumerateFiles(_base, "*.txt");
            foreach (var filename in files)
            {
                Console.WriteLine(filename);
                var text = File.ReadAllText(filename).Split(';');
                notes.Add(new Note
                {
                    NazwaPliku = filename,
                    Tekst = text[0].ToString().Replace("\n", " ").Replace("\r", " ").Substring(0, 22) + "...",
                    Data = File.GetCreationTime(filename)
                });
            }

            collectionView.ItemsSource = notes.OrderBy(d => d.Data).ToList();
        }

        public NotesPage()
        {
            InitializeComponent();
        }

        async void DodajClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(NewNote));
        }

        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                Note note = (Note)e.CurrentSelection.FirstOrDefault();

                ///data/user/0/com.companyname.gps_notes/files/.local/share/dz7btuzf.8gx.txt
                ///data/user/0/com.companyname.gps_notes/files/.local/share/dz7btuzf.8gx.txt
                Console.WriteLine(note.NazwaPliku);
                await Shell.Current.GoToAsync($"{nameof(NewNote)}?{nameof(NewNote.ItemId)}={note.NazwaPliku}");
            }
        }
    }
}