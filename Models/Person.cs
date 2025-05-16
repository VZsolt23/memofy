using SQLite;

namespace Memofy.Models
{
    public class Person
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string BirthdayString
        {
            get => Birthday.ToString("yyyy. MM. dd.");
            set => Birthday = DateOnly.Parse(value);
        }

        public string? NamedayString
        {
            get => Nameday?.ToString("yyyy. MM. dd.");
            set => Nameday = !string.IsNullOrWhiteSpace(value) ? DateOnly.Parse(value) : null;
        }

        [Ignore]
        public DateOnly Birthday { get; set; }

        [Ignore]
        public DateOnly? Nameday { get; set; }
    }
}
