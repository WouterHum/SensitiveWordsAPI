using SensitiveWordsAPI.Data;
using SensitiveWordsAPI.Models;
using Dapper;

namespace SensitiveWordsAPI.Repositories
{
    /// <summary>
    /// Repository for managing sensitive words in the database.
    /// </summary>
    public class SensitiveWordRepository : ISensitiveWordRepository
    {
        private readonly DapperContext _context;

        /// <summary>
        /// Initializes a new instance of the SensitiveWordRepository class with the specified Dapper context.
        /// </summary>
        /// <param name="context"></param>
        public SensitiveWordRepository(DapperContext context) => _context = context;

        /// <summary>
        /// Retrieves all sensitive words from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SensitiveWord>> GetAllAsync()
        {
            var query = "SELECT * FROM Sensitive_Words";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<SensitiveWord>(query);
        }

        /// <summary>
        ///  Retrieves a sensitive word by its ID from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SensitiveWord?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Sensitive_Words WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<SensitiveWord>(query, new { Id = id });
        }

        /// <summary>
        /// Creates a new sensitive word in the database.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(SensitiveWord word)
        {
            var query = "INSERT INTO Sensitive_Words (Word) VALUES (@Word); SELECT CAST(SCOPE_IDENTITY() as int)";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, word);
        }

        /// <summary>
        /// Updates an existing sensitive word in the database.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(SensitiveWord word)
        {
            var query = "UPDATE Sensitive_Words SET Word = @Word WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, word);
            return rows > 0;
        }

        /// <summary>
        /// Deletes a sensitive word from the database by its ID.   
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM Sensitive_Words WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, new { Id = id });
            return rows > 0;
        }
    }
}