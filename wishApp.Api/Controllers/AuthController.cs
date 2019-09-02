using System.Net;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using wishApp.Api.Data;
using wishApp.Api.Dtos;
using wishApp.Api.Models;
using AutoMapper;

namespace wishApp.Api.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] //?
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private  readonly IConfiguration _config;
        private readonly IAuthRepository _repo;
        private readonly IMapper _mapper;

        public AuthController(AppDbContext ctx, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config, IAuthRepository repo, IMapper mapper)
        {
            _ctx = ctx;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserForRegisterDto user)
        {   
            var userToCreate = _mapper.Map<User>(user);
            if(await _repo.UserExist(userToCreate.UserName)) 
            {
                return Unauthorized();
            }
            var userToReturn = await _repo.Register(userToCreate, user.Password);
            if(userToReturn != null)
                return Ok(userToReturn);
            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto user) // here can be something wrong
        {
            if(ModelState.IsValid) //dunnno it should be here
            {
                var userToReturn = await _repo.Login(user.UserName, user.Password, user.RememberMe, false);
                if (userToReturn != null)
                {
                    var tokenString = GenerateJSONWebToken(userToReturn);
                    return Ok(new { token = tokenString, user = userToReturn });
                }
            }
            return Unauthorized();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userInfo.UserName)
            };
            
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}