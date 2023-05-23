using System.ComponentModel.DataAnnotations;


namespace quizapi.Business_Logic_Layer.DTO
{
    public class UserRoleDTO
    {
        [Key]
        public int UserRoleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserRolesName { get; set; }
    }
}