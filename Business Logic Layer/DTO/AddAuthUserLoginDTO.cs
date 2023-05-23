using System.ComponentModel.DataAnnotations;




namespace quizapi.Business_Logic_Layer.DTO
{
    public class AddAuthUserLoginDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(60, ErrorMessage = "Password must be between 6 and 50 characters.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}