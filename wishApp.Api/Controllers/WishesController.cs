using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wishApp.Api.Data;
using wishApp.Api.Dtos;
using wishApp.Api.Models;

namespace wishApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishesController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly IWishRepository _wishRepo;
        private readonly AppDbContext _ctx;

        public WishesController(IUserRepository repo, IMapper mapper, IWishRepository wishRepo, AppDbContext ctx)
        {
            _repo = repo;
            _mapper = mapper;
            _wishRepo = wishRepo;
            _ctx = ctx;
        }

        [HttpGet]
        public async Task<IActionResult> getWishes()
        {
            var wishes = await _wishRepo.GetWishes();
            if (wishes != null)
                return Ok(wishes);
            return BadRequest("Problem with getting user wishes");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getWish(int id)
        {
            var wish = await _wishRepo.GetWish(id);
            if (wish != null)
                return Ok(wish);
            return BadRequest("Problem with getting user wish");

        }

        [HttpGet("user/{username}")]
        public async Task<IActionResult> getUserWishes(string userName)
        {
            var wishes = await _wishRepo.GetUserWishes(userName);
            if (wishes != null)
                return Ok(wishes);
            return BadRequest("Problem with getting user wish");

        }

        [Authorize]
        [HttpPost("user/{username}/add")]
        public async Task<IActionResult> addWish(WishForAddDto wish, string username)
        {
            var user = await _repo.GetUser(username);
            if (user.UserName.ToString() != User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                return Unauthorized();
            }
            var wishToAdd = _mapper.Map<Wish>(wish);
            _wishRepo.AddWish(user, wishToAdd);
            if (await _repo.SaveAll())
                return Ok(user);
            return BadRequest("Problem with adding new wish!");
        }

        [Authorize]
        [HttpDelete("user/{username}/{id}")]
        public async Task<IActionResult> deleteWish(string username, int id)
        {
            var user = await _repo.GetUser(username);
            if (user.UserName.ToString() != User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                return Unauthorized();
            }
            var wishToRemove = await _wishRepo.GetWish(id);
            user.UserWishes.Remove(wishToRemove);
            _wishRepo.RemoveWish(wishToRemove);
            if (await _repo.SaveAll())
                return Ok("Deleted");
            return BadRequest("Problem with removing wish");
        }

        [Authorize]
        [HttpPost("{id}/donate")]
        public async Task<IActionResult> DonateWish(int id, DonateDto donate)
        {
            var user = await _repo.GetUser(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var wish = await _wishRepo.GetWish(id); //zwroc z include user albo znajdz uzytkownika i dodaj mu to do salda co dostanie
            if (donate.Donate < user.Saldo)
            {
                user.Saldo -= donate.Donate;
                user.Costs += donate.Donate;
                wish.CollectedMoney += donate.Donate;
                wish.User.Saldo += donate.Donate;
                if (wish.CollectedMoney >= wish.Cost)
                    wish.IsFinished = true;
                if (await _repo.SaveAll())
                    return Ok("Wish Donated");
            }
            return BadRequest("Problem with donating");
        }

        // [Authorize]
        // [HttpPost("{wishId}")]
        // public async Task<IActionResult> payForWish(string name, int wishId)
        // {               
        //     var user = await _repo.GetUser(name);
        //     var wish = await _repo.GetUserWish(wishId);
        //     var loggedUsser = await _repo.GetUser(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        //     if(wish.Cost > loggedUsser.Saldo)
        //         return BadRequest("You cant affor this wish!");
        //     loggedUsser.Saldo -= wish.Cost; // if less than 0 return to previous saldo
        //     return Ok(wish);
        // }


    }
}