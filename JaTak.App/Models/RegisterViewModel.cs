using System.ComponentModel.DataAnnotations;

namespace JaTak.App.Models
{
    /// <summary>
    /// Holds the data from the create account form.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Gets or sets the email typed by the user.
        /// </summary>
        [Required(ErrorMessage = "Indtast din e-mail.")]
        [EmailAddress(ErrorMessage = "Indtast en gyldig e-mail.")]
        [StringLength(255)]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the phone number typed by the user.
        /// </summary>
        [Display(Name = "Telefonnummer")]
        [Required(ErrorMessage = "Indtast dit telefonnummer.")]
        [RegularExpression(@"^(\+45)?[0-9]{8}$",
            ErrorMessage = "Telefonnummeret skal være 8 cifre uden mellemrum.")]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password typed by the user.
        /// </summary>
        [Required(ErrorMessage = "Indtast en adgangskode.")]
        [StringLength(100, MinimumLength = 8,
            ErrorMessage = "Adgangskoden skal være mindst 8 tegn.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).+$",
            ErrorMessage = "Adgangskoden skal indeholde et stort bogstav, et lille bogstav og et tal.")]
        [DataType(DataType.Password)]
        [Display(Name = "Adgangskode")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the repeated password. It must match Password.
        /// </summary>
        [Display(Name = "Gentag adgangskode")]
        [Required(ErrorMessage = "Gentag adgangskoden.")]
        [Compare(nameof(Password), ErrorMessage = "De to adgangskoder er ikke ens.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}