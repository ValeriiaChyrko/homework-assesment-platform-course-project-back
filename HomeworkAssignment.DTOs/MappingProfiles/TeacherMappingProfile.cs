using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class TeacherMappingProfile : Profile
{
    public TeacherMappingProfile()
    {
        CreateMap<Teacher, RespondTeacherDto>()
            .ReverseMap();

        CreateMap<Teacher, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.GitHubProfiles, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Teacher, GitHubProfilesEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GitHubProfileId))
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Assignments, opt => opt.Ignore())
            .ForMember(dest => dest.Attempts, opt => opt.Ignore())
            .ReverseMap();
    }
}