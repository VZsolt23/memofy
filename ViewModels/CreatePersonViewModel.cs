using CommunityToolkit.Mvvm.ComponentModel;
using Memofy.Dtos;
using Memofy.Models;
using Memofy.Services;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Memofy.ViewModels
{
    public partial class CreatePersonViewModel : ObservableValidator
    {
        #region Form helper properties

        [ObservableProperty]
        private ObservableCollection<MonthOptionDto> monthsOfTheYear;

        [ObservableProperty]
        private ObservableCollection<int> daysForBirthday;

        [ObservableProperty]
        private ObservableCollection<int> daysForNameday;

        #endregion

        #region Person properties

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "A név megadása kötelező!")]
        private string personName;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "A hónap megadása kötelező!")]
        private MonthOptionDto? birthMonth;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "A nap megadása kötelező!")]
        private int? birthDay;

        [ObservableProperty]
        private MonthOptionDto? nameMonth;

        [ObservableProperty]
        private int? nameDay;

        #endregion

        public CreatePersonViewModel()
        {
            personName = string.Empty;

            monthsOfTheYear =
            [
                new() { Number = 1, Name = "Január" },
                new() { Number = 2, Name = "Február" },
                new() { Number = 3, Name = "Március" },
                new() { Number = 4, Name = "Április" },
                new() { Number = 5, Name = "Május" },
                new() { Number = 6, Name = "Június" },
                new() { Number = 7, Name = "Július" },
                new() { Number = 8, Name = "Augusztus" },
                new() { Number = 9, Name = "Szeptember" },
                new() { Number = 10, Name = "Október" },
                new() { Number = 11, Name = "November" },
                new() { Number = 12, Name = "December" }
            ];

            daysForBirthday = [.. Enumerable.Range(1, 31)];
            daysForNameday = [.. Enumerable.Range(1, 31)];
        }

        public async Task<bool> SavePersonAsync()
        {
            ValidateAllProperties();

            if (HasErrors)
            {
                OnPropertyChanged(nameof(NameError));
                OnPropertyChanged(nameof(BirthMonth));
                OnPropertyChanged(nameof(BirthDay));
                return false;
            }

            if (string.IsNullOrWhiteSpace(PersonName) || BirthMonth == null || DaysForBirthday == null)
                return false;

            var person = new Person
            {
                Name = PersonName,
                Birthday = new DateOnly(DateOnly.MinValue.Year, BirthMonth.Number, (int)BirthDay),
                Nameday = NameMonth != null && NameDay != null
                    ? new DateOnly(DateOnly.MinValue.Year, NameMonth.Number, NameDay.Value)
                    : null
            };

            await DatabaseService.Instance.InitAsync();
            await DatabaseService.Instance.AddPersonAsync(person);

            return true;
        }

        partial void OnPersonNameChanged(string value)
        {
            ValidateProperty(value, nameof(PersonName));
            OnPropertyChanged(nameof(NameError));
        }

        partial void OnBirthMonthChanged(MonthOptionDto? value)
        {
            if (value is not null)
                UpdateBirthDays(value.Number);

            ValidateProperty(value, nameof(BirthMonth));
            OnPropertyChanged(nameof(BirthMonth));
        }

        partial void OnBirthDayChanged(int? value)
        {
            ValidateProperty(value, nameof(BirthDay));
            OnPropertyChanged(nameof(BirthDay));
        }

        partial void OnNameMonthChanged(MonthOptionDto? value)
        {
            if (value is not null)
                UpdateNameDays(value.Number);
        }

        public string? NameError => GetErrors(nameof(PersonName))?.FirstOrDefault()?.ErrorMessage;
        public string? BirthMonthError => GetErrors(nameof(BirthMonth))?.FirstOrDefault()?.ErrorMessage;
        public string? BirthDayError => GetErrors(nameof(BirthDay))?.FirstOrDefault()?.ErrorMessage;

        private void UpdateDays(Func<ObservableCollection<int>> getter, Action<ObservableCollection<int>> setter, string propertyName, int month)
        {
            int dayCount = DateTime.DaysInMonth(DateTime.Now.Year, month);
            var newCollection = new ObservableCollection<int>(Enumerable.Range(1, dayCount));
            setter(newCollection);
            OnPropertyChanged(propertyName);
        }

        private void UpdateBirthDays(int month)
        {
            UpdateDays(() => DaysForBirthday, val => DaysForBirthday = val, nameof(DaysForBirthday), month);
        }

        private void UpdateNameDays(int month)
        {
            UpdateDays(() => DaysForNameday, val => DaysForNameday = val, nameof(DaysForNameday), month);
        }
    }
}
