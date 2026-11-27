using JaTak.App.Models;

namespace JaTak.Tests
{
    [TestFixture]
    public class OfferViewModelTests
    {
        [Test]
        public void Test01_StockColorClass_50PercentRemaining_ReturnsGreen()
        {
            // Arrange
            var offer = new OfferViewModel
            {
                Quantity = 100,
                QuantityLeft = 50
            };

            // Act
            var actual = offer.StockColorClass;

            // Assert
            Assert.That(actual, Is.EqualTo("stock-green"));
        }

        [Test]
        public void Test02_StockColorClass_49PercentRemaining_ReturnsOrange()
        {
            // Arrange
            var offer = new OfferViewModel
            {
                Quantity = 100,
                QuantityLeft = 49
            };

            // Act
            var actual = offer.StockColorClass;

            // Assert
            Assert.That(actual, Is.EqualTo("stock-orange"));
        }

        [Test]
        public void Test03_StockColorClass_25PercentRemaining_ReturnsOrange()
        {
            // Arrange
            var offer = new OfferViewModel
            {
                Quantity = 100,
                QuantityLeft = 25
            };

            // Act
            var actual = offer.StockColorClass;

            // Assert
            Assert.That(actual, Is.EqualTo("stock-orange"));
        }

        [Test]
        public void Test04_StockColorClass_24PercentRemaining_ReturnsRed()
        {
            // Arrange
            var offer = new OfferViewModel
            {
                Quantity = 100,
                QuantityLeft = 24
            };

            // Act
            var actual = offer.StockColorClass;

            // Assert
            Assert.That(actual, Is.EqualTo("stock-red"));
        }

        [Test]
        public void Test05_StockColorClass_NoStockRemaining_ReturnsRed()
        {
            // Arrange
            var offer = new OfferViewModel
            {
                Quantity = 100,
                QuantityLeft = 0
            };

            // Act
            var actual = offer.StockColorClass;

            // Assert
            Assert.That(actual, Is.EqualTo("stock-red"));
        }

        [Test]
        public void Test06_StockText_NormalStock_ReturnsRemainingQuantity()
        {
            // Arrange
            var offer = new OfferViewModel
            {
                Quantity = 100,
                QuantityLeft = 50
            };

            // Act
            var actual = offer.StockText;

            // Assert
            Assert.That(actual, Is.EqualTo("50 TILBAGE"));
        }

        [Test]
        public void Test07_StockText_LowStock_ReturnsLowStockText()
        {
            // Arrange
            var offer = new OfferViewModel
            {
                Quantity = 100,
                QuantityLeft = 10
            };

            // Act
            var actual = offer.StockText;

            // Assert
            Assert.That(actual, Is.EqualTo("KUN 10 TILBAGE!"));
        }

        [Test]
        public void Test08_StockText_NoStockRemaining_ReturnsSoldOutMessage()
        {
            // Arrange
            var offer = new OfferViewModel
            {
                Quantity = 100,
                QuantityLeft = 0
            };

            // Act
            var actual = offer.StockText;

            // Assert
            Assert.That(actual, Is.EqualTo("UDSOLGT"));
        }
    }
}