using Microsoft.Data.SqlClient;
using System.Data;

namespace SensitiveWordsAPI.Data
{
    /// <summary>
    /// DapperContext is a class that provides a connection to the database using Dapper.
    /// </summary>
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the DapperContext class with the specified configuration.
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        /// <summary>
        /// Creates a new database connection using the connection string.  
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
