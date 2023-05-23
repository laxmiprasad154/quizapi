using System.ComponentModel.DataAnnotations;

namespace quizapi.Data_Access_Layer.Entities
{
    public class UserRole
    {

        [Key]
        public int UserRoleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserRolesName { get; set; }
    }
}
