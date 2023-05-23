using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace quizapi.Data_Access_Layer.Entities
{
    public class User
    {


        [Key]

        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username must be between 1 and 50 characters.", MinimumLength = 1)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(60, ErrorMessage = "Password must be between 6 and 60 characters.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string FName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LName { get; set; }

        [Required]
        [ForeignKey("UserRole")]
        public int UserRoleId { get; set; }
        public virtual UserRole UserRole { get; set; }


    }
}
