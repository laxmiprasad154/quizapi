using System.ComponentModel.DataAnnotations;


﻿

namespace quizapi.Business_Logic_Layer.DTO
{
    public class AddAuthResponseDTO
    {

        public string token { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username must be between 1 and 50 characters.", MinimumLength = 1)]

        public int UserId { get; set; }
        public int role { get;set; }
    }
}