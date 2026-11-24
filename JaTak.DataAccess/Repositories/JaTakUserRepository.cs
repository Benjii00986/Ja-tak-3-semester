using JaTak.DataAccess.Connections;
using JaTak.DataAccess.Models;
using JaTak.DataAccess.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

// Forfatter: Andreas Larsen, Benyamin Mannan, Magnus Mortensen, Said Hamidi.
// Dato: 20-11-2026

namespace JaTak.DataAccess.Repositories
{
    /// <summary>
    /// Handles database operations related to JaTak users.
    /// </summary>
    public class JaTakUserRepository : IJaTakUserRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        /// <summary>
        /// Initializes a new instance of the JaTakUserRepository class.
        /// </summary>
        public JaTakUserRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Retrieves a user using their unique Keycloak ID.
        /// </summary>
        public async Task<JaTakUser?> GetByKeycloakIdAsync(string keycloakId)
        {
            using SqlConnection connection =
                _connectionFactory.CreateConnection();

            string query = """
                SELECT UserId, KeycloakId, Email, PhoneNumber, CreatedAt
                FROM JaTakUsers
                WHERE KeycloakId = @KeycloakId
                """;

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add(
                "@KeycloakId", SqlDbType.NVarChar, 255).Value = keycloakId;

            await connection.OpenAsync();

            using SqlDataReader reader =
                await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapUser(reader);
            }

            return null;
        }

        /// <summary>
        /// Retrieves a user using their email address.
        /// </summary>
        public async Task<JaTakUser?> GetByEmailAsync(string email)
        {
            using SqlConnection connection =
                _connectionFactory.CreateConnection();

            string query = """
                SELECT UserId, KeycloakId, Email, PhoneNumber, CreatedAt
                FROM JaTakUsers
                WHERE Email = @Email
                """;

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add(
                "@Email", SqlDbType.NVarChar, 256).Value = email;

            await connection.OpenAsync();

            using SqlDataReader reader =
                await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapUser(reader);
            }

            return null;
        }

        /// <summary>
        /// Creates a new JaTak user profile.
        /// Returns the generated UserId.
        /// </summary>
        public async Task<int> CreateAsync(JaTakUser user)
        {
            using SqlConnection connection =
                _connectionFactory.CreateConnection();

            string query = """
                INSERT INTO JaTakUsers
                    (KeycloakId, Email, PhoneNumber)
                OUTPUT INSERTED.UserId
                VALUES
                    (@KeycloakId, @Email, @PhoneNumber)
                """;

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add(
                "@KeycloakId", SqlDbType.NVarChar, 255)
                .Value = user.KeycloakId;

            command.Parameters.Add(
                "@Email", SqlDbType.NVarChar, 256)
                .Value = user.Email;

            command.Parameters.Add(
                "@PhoneNumber", SqlDbType.NVarChar, 32)
                .Value = (object?)user.PhoneNumber ?? DBNull.Value;

            await connection.OpenAsync();

            // Returns the automatically generated UserId.
            int userId = (int)await command.ExecuteScalarAsync();

            return userId;
        }

        /// <summary>
        /// Updates an existing JaTak user profile.
        /// </summary>
        public async Task UpdateAsync(JaTakUser user)
        {
            using SqlConnection connection =
                _connectionFactory.CreateConnection();

            string query = """
                UPDATE JaTakUsers
                SET Email = @Email,
                    PhoneNumber = @PhoneNumber
                WHERE UserId = @UserId
                """;

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add(
                "@UserId", SqlDbType.Int).Value = user.UserId;

            command.Parameters.Add(
                "@Email", SqlDbType.NVarChar, 256).Value = user.Email;

            command.Parameters.Add(
                "@PhoneNumber", SqlDbType.NVarChar, 32)
                .Value = (object?)user.PhoneNumber ?? DBNull.Value;

            await connection.OpenAsync();

            int rowsAffected = await command.ExecuteNonQueryAsync();

            Console.WriteLine($"User records updated: {rowsAffected}");
        }

        /// <summary>
        /// Maps database records to a JaTakUser object.
        /// </summary>
        private static JaTakUser MapUser(SqlDataReader reader)
        {
            return new JaTakUser
            {
                UserId = reader.GetInt32(
                    reader.GetOrdinal("UserId")),

                KeycloakId = reader["KeycloakId"].ToString()!,

                Email = reader["Email"].ToString()!,

                PhoneNumber = reader.IsDBNull(
                    reader.GetOrdinal("PhoneNumber"))
                    ? null
                    : reader["PhoneNumber"].ToString(),

                CreatedAt = reader.GetDateTime(
                    reader.GetOrdinal("CreatedAt"))
            };
        }
    }
}