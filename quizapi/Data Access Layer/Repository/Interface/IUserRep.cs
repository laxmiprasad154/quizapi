using quizapi.Data_Access_Layer.Entities;

namespace quizapi.Data_Access_Layer.Repository.Interface
{
    public interface IUserRep
    {
        Task<User> CreateAsync(User user);
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> UpdateAsync(int id, User user);
        Task<User> DeleteAsync(int id);
        Task<User> GetByEmailAsync(string email);
    }
}
