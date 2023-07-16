using CURD_APP.Data;
using CURD_APP.Models;
using CURD_APP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CURD_APP.Controllers
{
    [Route("api/v1")]
    public class UserController: ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtService _jwtService;

        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager, JwtService jwtService, SignInManager<IdentityUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _jwtService = jwtService;
            _signInManager = signInManager;
        }

        // POST: api/Users/BearerToken
        [HttpPost("/login")]
        public async Task<ActionResult<AuthenticationResponse>> CreateBearerToken([FromBody]Login request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad credentials");
            }
            request = new Login
            {
                UserName = "animesh1242",
                Password = "password123"
            };

            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                return BadRequest(new UserResponse<User>("Username Doesnt exist",new User { }));
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return BadRequest(new UserResponse<User>("Username Or Password is not correct",new User { }));
            }

            var token = _jwtService.CreateToken(user);

            return Ok(new UserResponse<AuthenticationResponse>("Logged in Successfully", token));
        }

        [HttpPost("/register")]
        public async Task<ActionResult<User>> PostUser([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new UserResponse<User>("Please Enter Valid Credential", new User { }));
            }

            var result = await _userManager.CreateAsync(
                new IdentityUser() { UserName = user.UserName, Email = user.Email },
                user.Password
            );

            if (!result.Succeeded)
            {
                return BadRequest(new UserResponse<User>("Server Error Occured Please Retry", new User { }));
            }

            user.Password = null;
            return Ok(new UserResponse<User>("Registration Successful", user));
        }
        [HttpPost("/logout")]
        //[Authorize]
        public async Task<ActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            
            await _jwtService.InvalidateToken(token);
            await _signInManager.SignOutAsync();
            return Ok(new UserResponse<User>("logout Successful", new User { }));
        }
        //[Authorize]
        [HttpGet("{username}")]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            IdentityUser user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound(new UserResponse<User>("Given user is not present", new User { }));
            }

            return Ok(new UserResponse<IdentityUser>("User fetched Successfully", user));
        }
    }

    public class UserResponse<T>
    {
        public string message { get; set; }
        public T data { get; set; }

        public UserResponse(string message,T? data)
        {
            this.message = message;
            this.data=data;
        }
    }

}
