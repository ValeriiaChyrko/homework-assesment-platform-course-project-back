using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs.ChapterRelated;

namespace HomeAssignment.DTOs.MappingProfiles;

public class UserChapterProgressMappingProfile : Profile
{
    public UserChapterProgressMappingProfile()
    {
        MapEntityToDomain();
        MapDomainToEntity();
        MapDomainToRespondDto();
    }

    private void MapEntityToDomain()
    {
        CreateMap<UserChapterProgressEntity, ChapterUserProgress>();
    }

    private void MapDomainToEntity()
    {
        CreateMap<ChapterUserProgress, UserChapterProgressEntity>()
            .ForMember(dest => dest.User, opt => opt.Ignore()) 
            .ForMember(dest => dest.Chapter, opt => opt.Ignore());
    }

    private void MapDomainToRespondDto()
    {
        CreateMap<ChapterUserProgress, RespondChapterUserProgressDto>();
    }
}