
using Memofy.Models;
using SQLite;

namespace Memofy.Services
{
    public class DatabaseService
    {
        private static SQLiteAsyncConnection _connection;
        private static readonly Lazy<DatabaseService> _instance = new(() => new DatabaseService());

        public static DatabaseService Instance => _instance.Value;

        private DatabaseService() { }

        public async Task InitAsync()
        {
            if (_connection != null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "people_events.db3");
            _connection = new SQLiteAsyncConnection(dbPath);

            await _connection.CreateTableAsync<Person>();
        }

        /// <summary>
        /// Add person with birthday and nameday
        /// </summary>
        /// <returns></returns>
        public async Task<int> AddPersonAsync(Person person)
        {
            return await _connection.InsertAsync(person);
        }

        /// <summary>
        /// Get all person from database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Person>> GetAllPersonsAsync()
        {
            return await _connection.Table<Person>().ToListAsync();
        }

        /// <summary>
        /// Get person by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Person?> GetPersonByIdAsync(int id)
        {
            return await _connection.Table<Person>().FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Update person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task<int> UpdatePersonAsync(Person person)
        {
            return await _connection.UpdateAsync(person);
        }

        /// <summary>
        /// Delete person
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePersonAsync(int id)
        {
            var person = await GetPersonByIdAsync(id);
            if (person is not null)
            {
                await _connection.DeleteAsync(person);
            }
        }
    }
}
