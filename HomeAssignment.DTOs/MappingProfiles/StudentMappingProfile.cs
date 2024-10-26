using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class StudentMappingProfile : Profile
{
    public StudentMappingProfile()
    {
        CreateMap<Student, RespondStudentDto>()
            .ReverseMap();
        
        CreateMap<Student, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId)) 
            .ReverseMap();
        
        CreateMap<Student, GitHubProfileDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GitHubProfileId)) 
            .ReverseMap();
    }
}