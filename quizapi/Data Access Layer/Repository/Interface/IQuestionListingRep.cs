using quizapi.Data_Access_Layer.Entities;

namespace quizapi.Data_Access_Layer.Repository.Interface
{
    public interface IQuestionListingRep
    {
        Task<Question> CreateAsync(Question Question);
        Task<List<Question>> GetAllAsync();
        Task<Question> GetByIdAsync(int id);
        Task<Question> UpdateAsync(int id, Question Question);
        Task<Question> DeleteAsync(int id);
    }
}
