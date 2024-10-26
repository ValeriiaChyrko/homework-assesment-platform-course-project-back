using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, UserEntity>().ReverseMap();
    }
}