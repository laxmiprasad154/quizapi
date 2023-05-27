using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace quizapi.Data_Access_Layer.Entities
{
    public class Question
    {
        [Key]
        public int QnId { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string QnInWords { get; set; }
      
        [Column(TypeName = "nvarchar(50)")]
        public string Option1 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Option2 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Option3 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Option4 { get; set; }

        public int Answer { get; set; }

        
       
    }
}
