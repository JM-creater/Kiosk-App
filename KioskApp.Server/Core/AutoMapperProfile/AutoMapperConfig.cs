using AutoMapper;
using KioskApp.Model.Dto;
using KioskApp.Model.Entities;

namespace KioskApp.Server.Core.AutoMapperProfile
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            // User
            CreateMap<RegisterUserDto, User>();
        }
    }
}
