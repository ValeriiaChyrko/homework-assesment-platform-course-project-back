using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class StudentMappingProfile : Profile
{
    public StudentMappingProfile()
    {
        CreateMap<Student, RespondStudentDto>()
            .ReverseMap();
        
        CreateMap<Student, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId)) 
            .ForMember(dest => dest.GitHubProfiles, opt => opt.Ignore()) 
            .ReverseMap();
        
        CreateMap<Student, GitHubProfilesEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GitHubProfileId)) 
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Assignments, opt => opt.Ignore())
            .ForMember(dest => dest.Attempts, opt => opt.Ignore())
            .ReverseMap();
    }
}