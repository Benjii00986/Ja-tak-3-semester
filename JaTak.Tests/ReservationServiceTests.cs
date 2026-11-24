using JaTak.BusinessLogic.Services;
using JaTak.DataAccess.Models;
using JaTak.DataAccess.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace JaTak.Tests.Services
{
    [TestFixture]
    public class ReservationServiceTests
    {
        private const string KeycloakId = "test-id";

        [Test]
        public async Task Test01_CreateReservation_ValidRequest_ReturnsReservation()
        {
            // Arrange
            var userRepository = new Mock<IJaTakUserRepository>();
            var offerRepository = new Mock<IOfferRepository>();
            var reservationRepository = new Mock<IReservationRepository>();

            userRepository
                .Setup(x => x.GetByKeycloakIdAsync(KeycloakId))
                .ReturnsAsync(new JaTakUser { UserId = 1 });

            offerRepository
                .Setup(x => x.GetOfferByIdAsync(1))
                .ReturnsAsync(new Offer { IsActive = true, QuantityLeft = 1 });

            reservationRepository
                .Setup(x => x.CreateReservationAsync(It.IsAny<Reservation>()))
                .ReturnsAsync((int?)1);

            var service = new ReservationService(
                reservationRepository.Object,
                offerRepository.Object,
                userRepository.Object);

            // Act
            var actual = await service.CreateReservationAsync(KeycloakId, 1, 1);

            // Assert
            Assert.That(actual.ReservationId, Is.EqualTo(1));
            Assert.That(actual.UserId, Is.EqualTo(1));
            Assert.That(actual.OfferId, Is.EqualTo(1));
            Assert.That(actual.Quantity, Is.EqualTo(1));
        }

        [Test]
        public void Test02_CreateReservation_EmptyKeycloakId_ThrowsArgumentException()
        {
            // Arrange
            var service = new ReservationService(
                Mock.Of<IReservationRepository>(),
                Mock.Of<IOfferRepository>(),
                Mock.Of<IJaTakUserRepository>());

            // Act
            AsyncTestDelegate act = async () =>
                await service.CreateReservationAsync("", 1, 1);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(act);
        }

        [Test]
        public void Test03_CreateReservation_ZeroQuantity_ThrowsArgumentException()
        {
            // Arrange
            var service = new ReservationService(
                Mock.Of<IReservationRepository>(),
                Mock.Of<IOfferRepository>(),
                Mock.Of<IJaTakUserRepository>());

            // Act
            AsyncTestDelegate act = async () =>
                await service.CreateReservationAsync(KeycloakId, 1, 0);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(act);
        }

        [Test]
        public void Test04_CreateReservation_NegativeQuantity_ThrowsArgumentException()
        {
            // Arrange
            var service = new ReservationService(
                Mock.Of<IReservationRepository>(),
                Mock.Of<IOfferRepository>(),
                Mock.Of<IJaTakUserRepository>());

            // Act
            AsyncTestDelegate act = async () =>
                await service.CreateReservationAsync(KeycloakId, 1, -1);

            // Assert
            Assert.ThrowsAsync<ArgumentException>(act);
        }

        [Test]
        public void Test05_CreateReservation_UserNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var userRepository = new Mock<IJaTakUserRepository>();

            userRepository
                .Setup(x => x.GetByKeycloakIdAsync(KeycloakId))
                .ReturnsAsync((JaTakUser?)null);

            var service = new ReservationService(
                Mock.Of<IReservationRepository>(),
                Mock.Of<IOfferRepository>(),
                userRepository.Object);

            // Act
            AsyncTestDelegate act = async () =>
                await service.CreateReservationAsync(KeycloakId, 1, 1);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(act);
        }

        [Test]
        public void Test06_CreateReservation_OfferNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var userRepository = new Mock<IJaTakUserRepository>();
            var offerRepository = new Mock<IOfferRepository>();

            userRepository
                .Setup(x => x.GetByKeycloakIdAsync(KeycloakId))
                .ReturnsAsync(new JaTakUser { UserId = 1 });

            offerRepository
                .Setup(x => x.GetOfferByIdAsync(1))
                .ReturnsAsync((Offer?)null);

            var service = new ReservationService(
                Mock.Of<IReservationRepository>(),
                offerRepository.Object,
                userRepository.Object);

            // Act
            AsyncTestDelegate act = async () =>
                await service.CreateReservationAsync(KeycloakId, 1, 1);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(act);
        }

        [Test]
        public void Test07_CreateReservation_InactiveOffer_ThrowsInvalidOperationException()
        {
            // Arrange
            var userRepository = new Mock<IJaTakUserRepository>();
            var offerRepository = new Mock<IOfferRepository>();

            userRepository
                .Setup(x => x.GetByKeycloakIdAsync(KeycloakId))
                .ReturnsAsync(new JaTakUser { UserId = 1 });

            offerRepository
                .Setup(x => x.GetOfferByIdAsync(1))
                .ReturnsAsync(new Offer { IsActive = false });

            var service = new ReservationService(
                Mock.Of<IReservationRepository>(),
                offerRepository.Object,
                userRepository.Object);

            // Act
            AsyncTestDelegate act = async () =>
                await service.CreateReservationAsync(KeycloakId, 1, 1);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(act);
        }

        [Test]
        public void Test08_CreateReservation_InsufficientQuantity_ThrowsInvalidOperationException()
        {
            // Arrange
            var userRepository = new Mock<IJaTakUserRepository>();
            var offerRepository = new Mock<IOfferRepository>();

            userRepository
                .Setup(x => x.GetByKeycloakIdAsync(KeycloakId))
                .ReturnsAsync(new JaTakUser { UserId = 1 });

            offerRepository
                .Setup(x => x.GetOfferByIdAsync(1))
                .ReturnsAsync(new Offer
                {
                    IsActive = true,
                    QuantityLeft = 1
                });

            var service = new ReservationService(
                Mock.Of<IReservationRepository>(),
                offerRepository.Object,
                userRepository.Object);

            // Act
            AsyncTestDelegate act = async () =>
                await service.CreateReservationAsync(KeycloakId, 1, 2);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(act);
        }

        [Test]
        public void Test09_CreateReservation_RepositoryReturnsNull_ThrowsInvalidOperationException()
        {
            // Arrange
            var userRepository = new Mock<IJaTakUserRepository>();
            var offerRepository = new Mock<IOfferRepository>();
            var reservationRepository = new Mock<IReservationRepository>();

            userRepository
                .Setup(x => x.GetByKeycloakIdAsync(KeycloakId))
                .ReturnsAsync(new JaTakUser { UserId = 1 });

            offerRepository
                .Setup(x => x.GetOfferByIdAsync(1))
                .ReturnsAsync(new Offer
                {
                    IsActive = true,
                    QuantityLeft = 1
                });

            reservationRepository
                .Setup(x => x.CreateReservationAsync(It.IsAny<Reservation>()))
                .ReturnsAsync((int?)null);

            var service = new ReservationService(
                reservationRepository.Object,
                offerRepository.Object,
                userRepository.Object);

            // Act
            AsyncTestDelegate act = async () =>
                await service.CreateReservationAsync(KeycloakId, 1, 1);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(act);
        }
    }
}