using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class ScoreSectionMappingProfile : Profile
{
    public ScoreSectionMappingProfile()
    {
        CreateMap<ScoreSection, ScoreSectionDto>()
            .ReverseMap();
    }
}