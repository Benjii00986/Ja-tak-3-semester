using JaTak.DataAccess.Connections;
using JaTak.DataAccess.Models;
using Microsoft.Data.SqlClient;

namespace JaTak.DataAccess.Repositories
{
    public class OfferRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public OfferRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Offer>> GetActiveOffersAsync()
        {
            List<Offer> offers = new List<Offer>();

            using SqlConnection connection =
                _connectionFactory.CreateConnection();

            await connection.OpenAsync();

            string query = """
                SELECT OfferId, ProductName, Description,
                ImageUrl, ShopName, Price, IsActive,
                PickupPoint, OriginalPrice, Quantity, QuantityLeft
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

                    OriginalPrice = reader.GetDecimal(reader.GetOrdinal("OriginalPrice")),

                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),

                    QuantityLeft = reader.GetInt32(reader.GetOrdinal("QuantityLeft"))
                };

                offers.Add(offer);
            }

            return offers;
        }
    }
}