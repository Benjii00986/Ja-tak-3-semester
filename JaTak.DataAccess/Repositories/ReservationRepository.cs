using JaTak.DataAccess.Connections;
using Microsoft.Data.SqlClient;

// Forfatter: Andreas Larsen, Benyamin Mannan, Magnus Mortensen, Said Hamidi.
// Dato: 24-11-2026

namespace JaTak.DataAccess.Repositories
{
    /// <summary>
    /// Handles database operations related to reservations.
    /// </summary>
    public class ReservationRepository
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
        public async Task<(string Status, int? ReservationId)> CreateReservationAsync(
            string keycloakId, int offerId, int quantity)
        {
            using SqlConnection connection =
                _connectionFactory.CreateConnection();

            await connection.OpenAsync();

            using SqlTransaction transaction =
                (SqlTransaction)await connection.BeginTransactionAsync();

            try
            {
                // Find the user using their Keycloak ID.
                string userQuery = """
                    SELECT UserId
                    FROM JaTakUsers
                    WHERE KeycloakId = @KeycloakId
                    """;

                using SqlCommand userCommand =
                    new SqlCommand(userQuery, connection, transaction);

                userCommand.Parameters.AddWithValue("@KeycloakId", keycloakId);

                object? userResult = await userCommand.ExecuteScalarAsync();

                if (userResult == null)
                {
                    await transaction.RollbackAsync();
                    return ("UserNotFound", null);
                }

                int userId = Convert.ToInt32(userResult);

                // Check whether the offer exists and is active.
                string offerQuery = """
                    SELECT COUNT(*)
                    FROM JaTakOffers
                    WHERE OfferId = @OfferId
                    AND IsActive = 1
                    """;

                using SqlCommand offerCommand =
                    new SqlCommand(offerQuery, connection, transaction);

                offerCommand.Parameters.AddWithValue("@OfferId", offerId);

                int offerExists = Convert.ToInt32(
                    await offerCommand.ExecuteScalarAsync());

                if (offerExists == 0)
                {
                    await transaction.RollbackAsync();
                    return ("OfferNotFound", null);
                }

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

                updateCommand.Parameters.AddWithValue("@Quantity", quantity);
                updateCommand.Parameters.AddWithValue("@OfferId", offerId);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    await transaction.RollbackAsync();
                    return ("InsufficientQuantity", null);
                }

                // Insert the reservation.
                string insertQuery = """
                    INSERT INTO JaTakReservations
                    (UserId, OfferId, Quantity, ReservedAt)
                    OUTPUT INSERTED.ReservationId
                    VALUES
                    (@UserId, @OfferId, @Quantity, GETDATE())
                    """;

                using SqlCommand insertCommand =
                    new SqlCommand(insertQuery, connection, transaction);

                insertCommand.Parameters.AddWithValue("@UserId", userId);
                insertCommand.Parameters.AddWithValue("@OfferId", offerId);
                insertCommand.Parameters.AddWithValue("@Quantity", quantity);

                int reservationId = Convert.ToInt32(
                    await insertCommand.ExecuteScalarAsync());

                await transaction.CommitAsync();

                return ("Success", reservationId);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}