using System.Net;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using wishApp.Api.Data;
using wishApp.Api.Dtos;
using wishApp.Api.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using wishApp.Api.Helpers;

namespace wishApp.Api.Controllers
{
    // [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        private readonly IMapper _mapper;
        private readonly IUserRepository _repo;

        public UsersController(AppDbContext ctx, IMapper mapper, IUserRepository repo)
        {
            _ctx = ctx;
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> getUsers([FromQuery]string currentFilter, string searchString, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var users = _repo.GetUsers();
            int pageSize = 15;

            if(users != null)
                return Ok(await PaginatedList<User>.CreateAsync(users, pageNumber ?? 1, pageSize));
            return BadRequest("Problem with getting all users");
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> getUser(string name) 
        {
            var user = await _repo.GetUser(name);
            if(user != null)
                return Ok(user);
            else
                return BadRequest();
        }

        [Authorize]
        [HttpPut("{name}")]
        public async Task<IActionResult> updateUser(UserForUpdatedDto userForUpdatedDto, string name)
        {
            var user = await _repo.GetUser(name.ToLower());
            if (user.UserName.ToString() != User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                return Unauthorized();
            }
            _mapper.Map(userForUpdatedDto, user);
            /*
                Configure it in angular with preload resolvers
             */
            if(await _repo.SaveAll())
                return Ok(user);
            return BadRequest("Problem with updating user");
        }
    }
}
