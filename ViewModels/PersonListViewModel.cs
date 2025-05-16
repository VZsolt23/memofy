using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Memofy.Dtos;
using Memofy.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Memofy.ViewModels
{
    public class PersonListViewModel : INotifyPropertyChanged
    {
        private static readonly SnackbarOptions _successSnackbar = new()
        {
            BackgroundColor = Colors.Green,
            TextColor = Colors.White,
            CornerRadius = new CornerRadius(12),
            ActionButtonTextColor = Colors.White
        };

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }

        public bool IsNotBusy => !IsBusy;


        public ObservableCollection<PersonDto> Persons { get; } = new();

        public ICommand DeleteCommand { get; }

        public PersonListViewModel()
        {
            DeleteCommand = new Command<int>(async (id) => await DeletePersonAsync(id));
        }

        public async Task LoadPersonsAsync()
        {
            IsBusy = true;
            try
            {
                await DatabaseService.Instance.InitAsync();
                var persons = await DatabaseService.Instance.GetAllPersonsAsync();

                Persons.Clear();

                foreach (var person in persons)
                {
                    Persons.Add(new PersonDto
                    {
                        Id = person.Id,
                        Name = person.Name,
                        Birthday = person.Birthday.ToString("MMMM dd."),
                        Nameday = person.Nameday?.ToString("MMMM dd.")
                    });
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task DeletePersonAsync(int id)
        {
            if (id == 0 || Application.Current?.MainPage == null)
            {
                await Toast.Make("Sikertelen törlés!").Show();
                return;
            }

            var answer = await ShowConfirmationPopup();
            if (!answer)
                return;

            await DatabaseService.Instance.InitAsync();
            await DatabaseService.Instance.DeletePersonAsync(id);

            await Snackbar.Make(
                message: "Sikeres törlés!",
                action: null,
                actionButtonText: string.Empty,
                visualOptions: _successSnackbar,
                duration: TimeSpan.FromSeconds(3)).Show();

            await LoadPersonsAsync();
        }

        private static async Task<bool> ShowConfirmationPopup()
        {
            return Application.Current?.MainPage != null && await Application.Current.MainPage.DisplayAlert(
                "Biztos, hogy törölni szeretnéd?",
                "Ez a művelet véglegesen törli a személy adatát.",
                "Igen",
                "Mégsem"
            );
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
