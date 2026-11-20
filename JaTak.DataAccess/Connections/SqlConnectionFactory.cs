using Microsoft.Data.SqlClient;

// Forfatter: Andreas Larsen, Benyamin Mannan, Magnus Mortensen, Said Hamidi.
// Dato: 20-11-2026

namespace JaTak.DataAccess.Connections
{

    /// <summary>
    /// Creates SQL database connections using the configured connection string.
    /// </summary>
    public class SqlConnectionFactory
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the SqlConnectionFactory class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string used to connect to the database.
        /// </param>
        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Creates a new SQL database connection.
        /// </summary>
        /// <returns>
        /// A new SqlConnection configured with the stored connection string.
        /// </returns>
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}