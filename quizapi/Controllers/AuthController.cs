using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using quizapi.Business_Logic_Layer.DTO;
using quizapi.Data_Access_Layer.Entities;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using quizapi.Data_Access_Layer.context;
using quizapi.Data_Access_Layer.Repository;
using quizapi.Infrastructure;
using IdentityModel;

namespace quizapi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private Quizdbcontext context;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IUserRep userRepo;

        public AuthController(Quizdbcontext context, IConfiguration configuration, IMapper mapper, IUserRep userRepo)
        {
            this.context = context;
            this.configuration = configuration;
            this.mapper = mapper;
            this.userRepo = userRepo;

        }
        [Route("login")]
        [HttpPost]
        public IActionResult Login(AddAuthUserLoginDTO loginModel)
        {

            var user = context.Users.Include(x => x.UserRoleId).SingleOrDefault(x => x.Email == loginModel.Email);

            if (user is null)
                return Unauthorized("Invalid Username or Password!");

            string hashedPassword = HashPassword(loginModel.Password);
            if (BCrypt.Net.BCrypt.Verify(loginModel.Password, user.Password))
            {

                var token = JWT.GenerateToken(new Dictionary<string, string> {
                { ClaimTypes.Role, user.UserRole.UserRolesName  },
                { "RoleId", user.UserRole.UserRoleId.ToString() },
                { JwtClaimTypes.PreferredUserName, user.UserName },
                { JwtClaimTypes.Id, user.UserId.ToString() },
                { JwtClaimTypes.Email, user.Email}
            }, configuration["JWT:Key"]);



                return Ok(new AddAuthResponseDTO { token = token, UserName = user.UserName });
            }
            else
            {
                return Unauthorized("Invalid Username or Password");
            }
        }
        [Route("Registration")]
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] AddUserRequestDTO addUserRequestDTO)
        {

            // Check if a user with the same email already exists
            var existingUser = await userRepo.GetByEmailAsync(addUserRequestDTO.Email);
            if (existingUser != null)
            {
                // Return an error response indicating that the email is already registered
                return BadRequest("Email is already registered.");
            }
            //Map DTO to Domain Model           
            var userEntity = mapper.Map<User>(addUserRequestDTO);
            userEntity.Password = HashPassword(addUserRequestDTO.Password);



            await userRepo.CreateAsync(userEntity);
            // var users = mapper.Map<UserDTO>(userEntity);

            return Ok("Registration Successful");
        }
        private string HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }
    }
}
