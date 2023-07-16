using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using CURD_APP.Controllers;
using CURD_APP.Models;
using CURD_APP.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using CURD_APP.Services;
using Microsoft.AspNetCore.Identity;
using Azure.Core;

namespace FunctionApp4
{
    public class UserRoute
    {
        private readonly ILogger _logger;
        private IAPIServices _services;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtService _jwtService;
        public UserRoute(ApplicationDbContext db, UserManager<IdentityUser> userManager, JwtService jwtService, SignInManager<IdentityUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _jwtService = jwtService;
            _signInManager = signInManager;
        }

        [Function("Register")]
        public async Task<User> Register([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            User user = JsonConvert.DeserializeObject<User>(requestBody);
            //User user = req;
            var result = await _userManager.CreateAsync(
                new IdentityUser() { UserName = user.UserName, Email = user.Email },
                user.Password
            );
            user.Password = null;
            return user;
        }
        [Function("Login")]
        public async Task<IdentityUser> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Login request = JsonConvert.DeserializeObject<Login>(requestBody);
            var user = await _userManager.FindByNameAsync(request.UserName);
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            //var token = _jwtService.CreateToken(user);
            return user;
        }

    }
}
