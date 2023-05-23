using AutoMapper;
using quizapi.Business_Logic_Layer.DTO;
using quizapi.Data_Access_Layer.Entities;





namespace quizapi.Infrastructure
{
    public class AutoMapperProfiles : Profile

    {
        public AutoMapperProfiles()
        {
            CreateMap<AddUserRequestDTO, User>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserRole, UserRoleDTO>().ReverseMap();
            CreateMap<UpdateUserRequestDTO, User>().ReverseMap();
            CreateMap<AddQuestionRequestDTO, Question>().ReverseMap();
            CreateMap<Question, QuestionDTO>().ReverseMap();
            CreateMap<UpdateQuestionDTO, Question>().ReverseMap();
            
            
        }
    }
}