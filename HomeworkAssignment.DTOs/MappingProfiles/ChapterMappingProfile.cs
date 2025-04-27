using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs.ChapterRelated;
using HomeAssignment.DTOs.RespondDTOs.ChapterRelated;

namespace HomeAssignment.DTOs.MappingProfiles;

public class ChapterMappingProfile : Profile
{
    public ChapterMappingProfile()
    {
        MapRequestToDomain();
        MapEntityToDomain();
        MapDomainToEntity();
        MapDomainToRespondDto();
    }

    private void MapRequestToDomain()
    {
        CreateMap<RequestCreateChapterDto, Chapter>()
            .ConstructUsing(src => Chapter.Create(src.Title));
    }

    private void MapEntityToDomain()
    {
        CreateMap<ChapterEntity, Chapter>()
            .ConstructUsing(src => new Chapter(
                src.Id,
                src.Title,
                src.Description,
                src.VideoUrl,
                src.Position,
                src.IsPublished,
                src.IsFree,
                src.CourseId,
                src.Attachments != null ? src.Attachments.Select(a => a.Id).ToList() : null,
                src.UsersProgress != null ? src.UsersProgress.Select(a => a.Id).ToList() : null,
                src.CreatedAt,
                src.UpdatedAt
            ));
    }

    private void MapDomainToEntity()
    {
        CreateMap<Chapter, ChapterEntity>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore())
            .ForMember(dest => dest.UsersProgress, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
    }

    private void MapDomainToRespondDto()
    {
        CreateMap<Chapter, RespondChapterDto>();
    }
}