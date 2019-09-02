using AutoMapper;
using wishApp.Api.Dtos;
using wishApp.Api.Models;

namespace wishApp.Api.Helpers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserForUpdatedDto, User>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<UserForLoginDto, User>();
            CreateMap<WishForAddDto, Wish>();
        }
    }
}