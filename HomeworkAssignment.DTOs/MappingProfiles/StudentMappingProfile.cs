using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class StudentMappingProfile : Profile
{
    public StudentMappingProfile()
    {
        CreateMap<Student, RespondStudentDto>();

        CreateMap<Student, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
            .ReverseMap();

        CreateMap<Student, RespondStudentDto>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash));
        
        CreateMap<UserDto, RespondStudentDto>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

        CreateMap<Student, GitHubProfileDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GitHubProfileId))
            .ReverseMap();
    }
}