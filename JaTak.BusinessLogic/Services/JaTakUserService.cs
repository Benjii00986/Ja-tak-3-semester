using JaTak.DataAccess.Models;
using JaTak.DataAccess.Repositories;

// Forfatter: Andreas Larsen, Benyamin Mannan, Magnus Mortensen, Said Hamidi.
// Dato: 20-11-2026

namespace JaTak.BusinessLogic.Services
{
    /// <summary>
    /// Handles business logic related to JaTak users.
    /// </summary>
    public class JaTakUserService
    {
        private readonly JaTakUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the JaTakUserService class.
        /// </summary>
        public JaTakUserService(JaTakUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Retrieves an existing JaTak user or creates a new profile
        /// if the user does not already exist.
        /// Updates the user's information if it has changed.
        /// </summary>
        public async Task<JaTakUser> GetOrCreateUserAsync(
            string keycloakId,
            string email,
            string? phoneNumber)
        {
            // Checks that the required user information is available.
            if (string.IsNullOrWhiteSpace(keycloakId))
            {
                throw new ArgumentException(
                    "Keycloak ID kan ikke være tomt.", nameof(keycloakId));
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException(
                    "Email kan ikke være tomt.", nameof(email));
            }

            // Checks whether the user already exists using their Keycloak ID.
            JaTakUser? existingUser =
                await _userRepository.GetByKeycloakIdAsync(keycloakId);

            if (existingUser != null)
            {
                // Updates the existing user's information if it has changed.
                if (existingUser.Email != email ||
                    existingUser.PhoneNumber != phoneNumber)
                {
                    existingUser.Email = email;
                    existingUser.PhoneNumber = phoneNumber;

                    await _userRepository.UpdateAsync(existingUser);
                }

                return existingUser;
            }

            // Checks whether the email is already connected to another account.
            JaTakUser? existingEmailUser =
                await _userRepository.GetByEmailAsync(email);

            if (existingEmailUser != null)
            {
                throw new InvalidOperationException(
                    "Denne e-mailadresse er allerede tilknyttet en JaTak-konto.");
            }

            // Creates a new JaTak user profile.
            JaTakUser newUser = new JaTakUser
            {
                KeycloakId = keycloakId,
                Email = email,
                PhoneNumber = phoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            // Saves the new user and retrieves the generated database ID.
            newUser.UserId =
                await _userRepository.CreateAsync(newUser);

            return newUser;
        }
    }
}