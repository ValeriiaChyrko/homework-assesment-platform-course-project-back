using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, UserEntity>().ReverseMap();
        CreateMap<GitHubProfileDto, GitHubProfilesEntity>().ReverseMap();

        CreateMap<ScoreSectionDto, ScoreSection>().ReverseMap();
    }
}