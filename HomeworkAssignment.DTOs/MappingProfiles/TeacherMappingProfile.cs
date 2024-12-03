using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class TeacherMappingProfile : Profile
{
    public TeacherMappingProfile()
    {
        CreateMap<Teacher, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
            .ReverseMap();

        CreateMap<Teacher, RespondTeacherDto>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash));

        CreateMap<UserDto, RespondTeacherDto>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

        CreateMap<Teacher, GitHubProfileDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GitHubProfileId))
            .ReverseMap();
    }
}