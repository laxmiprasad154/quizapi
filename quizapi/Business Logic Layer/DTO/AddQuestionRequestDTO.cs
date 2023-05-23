namespace quizapi.Business_Logic_Layer.DTO
{
    public class AddQuestionRequestDTO
    {
        public int QnId { get; set; }


        public string QnInWords { get; set; }


        public string Option1 { get; set; }
        public string Option2 { get; set; }

        public string Option3 { get; set; }
        public string Option4 { get; set; }


        public string Answer { get; set; }
    }
}
