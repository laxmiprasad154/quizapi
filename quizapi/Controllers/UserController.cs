using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using quizapi.Business_Logic_Layer.DTO;
using quizapi.Data_Access_Layer.Entities;
using AutoMapper;
using quizapi.Data_Access_Layer.Repository.Interface;
using IdentityModel;
using quizapi.Data_Access_Layer.context;
using quizapi.Infrastructure;
using System.Security.Claims;

using Microsoft.EntityFrameworkCore;



namespace quizapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class UserController : ControllerBase
    {
        private Quizdbcontext context;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IUserRep userRepo;
        

        public UserController(IMapper mapper, IUserRep userRepo,Quizdbcontext context,IConfiguration configuration)
        {
            this.mapper = mapper;
            this.userRepo = userRepo;
            this.context = context;
            this.configuration = configuration;

            

        }

        //Create User
        //Post:/api/users        


        //GET Walks        //GET:/api/walks       
        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var userEntity = await userRepo.GetAllAsync();

            return Ok(mapper.Map<List<UserDTO>>(userEntity));
        }

        [HttpGet]
        [Route("{id:int}/admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var userEntity = await userRepo.GetByIdAsync(id);
            if (userEntity == null)
            {
                return NotFound();
            }
            var users = mapper.Map<UserDTO>(userEntity);
            return Ok(users);

        }

       /* [HttpPut]
        [Route("{id:int}")]


        public async Task<IActionResult> Update([FromRoute] int id, UpdateUserRequestDTO updateUserRequestDTO)
        {
            var userEntity = mapper.Map<User>(updateUserRequestDTO);
            userEntity = await userRepo.UpdateAsync(id, userEntity);
            if (userEntity == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<UserDTO>(userEntity));
        }
       */

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, UserResultDTO updateUserRequestDTO)
        {
           

            var existingUser = await context.Users.FindAsync(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Score = updateUserRequestDTO.Score;
            existingUser.TimeTaken = updateUserRequestDTO.TimeTaken;

            context.Entry(existingUser).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        private bool UserExists(int id)
        {
            return context.Users.Any(e => e.UserId == id);
        }



        [HttpDelete]
        [Route("{id:int}/admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var entityDeleted = await userRepo.DeleteAsync(id);
            if (entityDeleted == null)
            {
                return NotFound();
            }

            return NoContent();
        }
       


            [Route("login")]
            [HttpPost]
            [AllowAnonymous]
            public IActionResult Login(AddAuthUserLoginDTO loginModel)
            {

                var user = context.Users.Include(x => x.UserRole).FirstOrDefault(x => x.Email == loginModel.Email);




                if (user is null)
                    return Unauthorized("Invalid Username or Password!");

                string hashedPassword = HashPassword(loginModel.Password);
                if (BCrypt.Net.BCrypt.Verify(loginModel.Password, hashedPassword))
                {

                    var token = JWT.GenerateToken(new Dictionary<string, string> {
                { ClaimTypes.Role, user.UserRole.UserRolesName  },
                { "RoleId", user.UserRole.UserRoleId.ToString() },
                {JwtClaimTypes.PreferredUserName, user.UserName },
                { JwtClaimTypes.Id, user.UserId.ToString() },
                { JwtClaimTypes.Email, user.Email}
            }, configuration["JWT:Key"]);



                    return Ok(new AddAuthResponseDTO { token = token, UserId = user.UserId, role = user.UserRoleId });
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



