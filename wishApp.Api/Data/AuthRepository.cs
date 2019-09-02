using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using wishApp.Api.Models;

namespace wishApp.Api.Data
{
    public class AuthRepository : IAuthRepository
    {
        private SignInManager<User> _signInManager;
        private IUserRepository _userRepo;
        private readonly UserManager<User> _userManager;

        public AuthRepository(SignInManager<User> signInManager, IUserRepository userRepo, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userRepo = userRepo;
            _userManager = userManager;
        }

        public async Task<User> Login(string username, string password, bool rememberMe, bool lockoutOnFailure)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure);
            if(result.Succeeded) {
                return await _userRepo.GetUser(username);
            }
            return null;
        }

        public async Task<User> Register(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if(result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
            }
            return user;

        }

        public async Task<bool> UserExist(string username)
        {
            var userFromRepo = await _userRepo.GetUser(username);
            if(userFromRepo != null)
                return true;
            return false;
        }
    }
}