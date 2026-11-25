using JaTak.DataAccess.Connections;
using JaTak.DataAccess.Models;
using JaTak.DataAccess.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

// Forfatter: Andreas Larsen, Benyamin Mannan, Magnus Mortensen, Said Hamidi.
// Dato: 24-11-2026

namespace JaTak.DataAccess.Repositories
{
    /// <summary>
    /// Handles database operations related to reservations.
    /// </summary>
    public class ReservationRepository : IReservationRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        /// <summary>
        /// Initializes a new instance of the ReservationRepository class.
        /// </summary>
        /// <param name="connectionFactory">
        /// Factory used to create SQL database connections.
        /// </param>
        public ReservationRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Creates a reservation and updates the available offer quantity.
        /// Both operations are performed within a SQL transaction.
        /// </summary>
        /// <param name="reservation">
        /// The reservation containing the user, offer and requested quantity.
        /// </param>
        /// <returns>
        /// The generated reservation ID if successful; otherwise, null.
        /// </returns>
        public async Task<int?> CreateReservationAsync(Reservation reservation)
        {
            using SqlConnection connection =
                _connectionFactory.CreateConnection();

            await connection.OpenAsync();

            using SqlTransaction transaction =
                (SqlTransaction)await connection.BeginTransactionAsync();

            try
            {
                // Decrease available quantity if enough products remain.
                string updateQuery = """
                    UPDATE JaTakOffers
                    SET QuantityLeft = QuantityLeft - @Quantity
                    WHERE OfferId = @OfferId
                    AND IsActive = 1
                    AND QuantityLeft >= @Quantity
                    """;

                using SqlCommand updateCommand =
                    new SqlCommand(updateQuery, connection, transaction);

                updateCommand.Parameters.AddWithValue(
                    "@Quantity", reservation.Quantity);

                updateCommand.Parameters.AddWithValue(
                    "@OfferId", reservation.OfferId);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                // Insert the reservation and retrieve its generated ID.
                string insertQuery = """
                    INSERT INTO JaTakReservations
                    (UserId, OfferId, Quantity, ReservedAt)
                    OUTPUT INSERTED.ReservationId
                    VALUES
                    (@UserId, @OfferId, @Quantity, SYSUTCDATETIME())
                    """;

                using SqlCommand insertCommand =
                    new SqlCommand(insertQuery, connection, transaction);

                insertCommand.Parameters.AddWithValue(
                    "@UserId", reservation.UserId);

                insertCommand.Parameters.AddWithValue(
                    "@OfferId", reservation.OfferId);

                insertCommand.Parameters.AddWithValue(
                    "@Quantity", reservation.Quantity);

                int reservationId = Convert.ToInt32(
                    await insertCommand.ExecuteScalarAsync());

                await transaction.CommitAsync();

                return reservationId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Retrieves all reservations belonging to a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of the user's reservations.</returns>
        public async Task<List<ReservationOverview>> GetReservationsByUserIdAsync(int userId)
        {
            List<ReservationOverview> reservations = new List<ReservationOverview>();

            using SqlConnection connection =
                _connectionFactory.CreateConnection();

            await connection.OpenAsync();

            string query = """
                SELECT
                    r.ReservationId,
                    o.ProductName,
                    o.PickupEndDateTime
                FROM JaTakReservations r
                INNER JOIN JaTakOffers o ON r.OfferId = o.OfferId
                WHERE r.UserId = @UserId
                ORDER BY r.ReservedAt DESC
                """;

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserId", userId);

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                ReservationOverview reservation = new ReservationOverview
                {
                    ReservationId = reader.GetInt32(reader.GetOrdinal("ReservationId")),
                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                    PickupEndDateTime = reader.IsDBNull(reader.GetOrdinal("PickupEndDateTime"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("PickupEndDateTime"))
                };

                reservation.IsCompleted =
                    reservation.PickupEndDateTime.HasValue &&
                    reservation.PickupEndDateTime.Value < DateTime.Now;

                reservations.Add(reservation);
            }

            return reservations;
        }
    }
}