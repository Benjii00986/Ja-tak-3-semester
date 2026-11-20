using JaTak.DataAccess.Connections;
using JaTak.DataAccess.Models;
using Microsoft.Data.SqlClient;

// Forfatter: Andreas Larsen, Benyamin Mannan, Magnus Mortensen, Said Hamidi.
// Dato: 20-11-2026

namespace JaTak.DataAccess.Repositories
{
    /// <summary>
    /// Handles database operations related to offers.
    /// Retrieves offer data from the JaTakOffers table.
    /// </summary>
    public class OfferRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        /// <summary>
        /// Initializes a new instance of the OfferRepository class.
        /// </summary>
        /// <param name="connectionFactory">
        /// Factory used to create SQL database connections.
        /// </param>
        public OfferRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Retrieves all active offers from the database.
        /// The offers are ordered by their unique ID.
        /// </summary>
        /// <returns>
        /// A list containing all active offers.
        /// Returns an empty list if no active offers are found.
        /// </returns>
        public async Task<List<Offer>> GetActiveOffersAsync()
        {
            List<Offer> offers = new List<Offer>();

            using SqlConnection connection =
                _connectionFactory.CreateConnection();

            await connection.OpenAsync();

            string query = """
                SELECT OfferId, ProductName, Description,
                ImageUrl, ShopName, Price, IsActive,
                PickupPoint, PickupStartDateTime, PickupEndDateTime,
                OriginalPrice, Quantity, QuantityLeft
                FROM JaTakOffers
                WHERE IsActive = 1
                ORDER BY OfferId
                """;

            using SqlCommand command =
                new SqlCommand(query, connection);

            using SqlDataReader reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Offer offer = new Offer
                {
                    OfferId = reader.GetInt32(
                        reader.GetOrdinal("OfferId")),

                    ProductName = reader["ProductName"].ToString()!,

                    Description = reader["Description"].ToString()!,

                    ImageUrl = reader["ImageUrl"].ToString()!,

                    ShopName = reader["ShopName"].ToString()!,

                    Price = reader.GetDecimal(
                        reader.GetOrdinal("Price")),

                    IsActive = reader.GetBoolean(
                        reader.GetOrdinal("IsActive")),

                    PickupPoint = reader["PickupPoint"].ToString()!,

                    // Retrieves the pickup start date and time, handling NULL values.
                    PickupStartDateTime = reader.IsDBNull(
                        reader.GetOrdinal("PickupStartDateTime"))
                        ? null
                        : reader.GetDateTime(
                            reader.GetOrdinal("PickupStartDateTime")),

                    // Retrieves the pickup end date and time, handling NULL values.
                    PickupEndDateTime = reader.IsDBNull(
                        reader.GetOrdinal("PickupEndDateTime"))
                        ? null
                        : reader.GetDateTime(
                            reader.GetOrdinal("PickupEndDateTime")),

                    OriginalPrice = reader.GetDecimal(
                        reader.GetOrdinal("OriginalPrice")),

                    Quantity = reader.GetInt32(
                        reader.GetOrdinal("Quantity")),

                    QuantityLeft = reader.GetInt32(
                        reader.GetOrdinal("QuantityLeft"))
                };

                offers.Add(offer);
            }

            return offers;
        }

        /// <summary>
        /// Retrieves a specific offer from the database using its ID.
        /// </summary>
        /// <param name="offerId">The unique ID of the offer to retrieve.</param>
        /// <returns>
        /// The matching offer if found; otherwise, null.
        /// </returns>
        public async Task<Offer?> GetOfferByIdAsync(int offerId)
        {
            Offer? foundOffer = null;

            using SqlConnection connection =
                _connectionFactory.CreateConnection();

            string query = """
                SELECT OfferId, ProductName, Description,
                ImageUrl, ShopName, Price, IsActive,
                PickupPoint, PickupStartDateTime, PickupEndDateTime,
                OriginalPrice, Quantity, QuantityLeft
                FROM JaTakOffers
                WHERE OfferId = @OfferId
                """;

            using SqlCommand command =
                new SqlCommand(query, connection);

            SqlParameter idParam =
                new SqlParameter("@OfferId", offerId);

            command.Parameters.Add(idParam);

            await connection.OpenAsync();

            using SqlDataReader reader =
                await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                foundOffer = new Offer
                {
                    OfferId = reader.GetInt32(
                        reader.GetOrdinal("OfferId")),

                    ProductName = reader["ProductName"].ToString()!,

                    Description = reader["Description"].ToString()!,

                    ImageUrl = reader["ImageUrl"].ToString()!,

                    ShopName = reader["ShopName"].ToString()!,

                    Price = reader.GetDecimal(
                        reader.GetOrdinal("Price")),

                    IsActive = reader.GetBoolean(
                        reader.GetOrdinal("IsActive")),

                    PickupPoint = reader["PickupPoint"].ToString()!,

                    // Retrieves the pickup start date and time, handling NULL values.
                    PickupStartDateTime = reader.IsDBNull(
                        reader.GetOrdinal("PickupStartDateTime"))
                        ? null
                        : reader.GetDateTime(
                            reader.GetOrdinal("PickupStartDateTime")),

                    // Retrieves the pickup end date and time, handling NULL values.
                    PickupEndDateTime = reader.IsDBNull(
                        reader.GetOrdinal("PickupEndDateTime"))
                        ? null
                        : reader.GetDateTime(
                            reader.GetOrdinal("PickupEndDateTime")),

                    OriginalPrice = reader.GetDecimal(
                        reader.GetOrdinal("OriginalPrice")),

                    Quantity = reader.GetInt32(
                        reader.GetOrdinal("Quantity")),

                    QuantityLeft = reader.GetInt32(
                        reader.GetOrdinal("QuantityLeft"))
                };
            }

            return foundOffer;
        }
    }
}