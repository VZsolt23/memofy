using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Memofy.ViewModels
{
    public class CreatePersonViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MonthOption> Months { get; set; }
        public ObservableCollection<int> BirthDays { get; set; }
        public ObservableCollection<int> NameDays { get; set; }

        #region Birthday props

        private MonthOption _birthMonth;
        public MonthOption BirthMonth
        {
            get => _birthMonth;
            set
            {
                if (_birthMonth != value)
                {
                    _birthMonth = value;
                    OnPropertyChanged();
                    UpdateBirthDays(value.Number);
                }
            }
        }

        private int _birthDay;
        public int BirthDay
        {
            get => _birthDay;
            set
            {
                if (_birthDay != value)
                {
                    _birthDay = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Nameday props

        private MonthOption _namedayMonth { get; set; }
        public MonthOption NamedayMonth
        {
            get => _namedayMonth;
            set
            {
                if (_namedayMonth != value)
                {
                    _namedayMonth = value;
                    OnPropertyChanged();
                    UpdateNameDays(value.Number);
                }
            }
        }

        private int _namedayDay { get; set; }
        public int NamedayDay
        {
            get => _namedayDay;
            set
            {
                if (_namedayDay != value)
                {
                    _namedayDay = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isNamedayEnabled;
        public bool IsNamedayEnabled
        {
            get => _isNamedayEnabled;
            set
            {
                if (_isNamedayEnabled != value)
                {
                    _isNamedayEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        public CreatePersonViewModel()
        {
            Months =
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

            BirthDays = [.. Enumerable.Range(1, 31)];
            NameDays = [.. Enumerable.Range(1, 31)];
        }

        private void UpdateDays(Func<ObservableCollection<int>> getter, Action<ObservableCollection<int>> setter, string propertyName, int month)
        {
            int dayCount = DateTime.DaysInMonth(DateTime.Now.Year, month);
            var newCollection = new ObservableCollection<int>(Enumerable.Range(1, dayCount));
            setter(newCollection);
            OnPropertyChanged(propertyName);
        }

        private void UpdateBirthDays(int month)
        {
            UpdateDays(() => BirthDays, val => BirthDays = val, nameof(BirthDays), month);
        }

        private void UpdateNameDays(int month)
        {
            UpdateDays(() => NameDays, val => NameDays = val, nameof(NameDays), month);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class MonthOption
    {
        public int Number { get; set; }      // 1–12
        public required string Name { get; set; }     // "Január", "Február" stb.
    }
}
