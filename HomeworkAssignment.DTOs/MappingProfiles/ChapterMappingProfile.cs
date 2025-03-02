using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class ChapterMappingProfile : Profile
{
    public ChapterMappingProfile()
    {
        CreateMap<RequestChapterDto, Chapter>()
            .ConstructUsing(src => Chapter.Create(
                src.Title,
                src.Description,
                src.VideoUrl,
                src.Position,
                src.IsPublished,
                src.IsFree,
                src.MuxDataId,
                src.CourseId,
                null,
                null
            ));
        
        CreateMap<ChapterEntity, Chapter>()
            .ConstructUsing(src => new Chapter(
                src.Id,
                src.Title,
                src.Description,
                src.VideoUrl,
                src.Position,
                src.IsPublished,
                src.IsFree,
                src.MuxDataId,
                src.CourseId,
                src.Attachments != null ? src.Attachments.Select(a => a.Id).ToList() : null,
                src.UsersProgress != null ? src.UsersProgress.Select(up => up.Id).ToList() : null,
                src.CreatedAt,
                src.UpdatedAt
            ));
        
        CreateMap<Chapter, ChapterEntity>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore())
            .ForMember(dest => dest.UsersProgress, opt => opt.Ignore()) 
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        
        CreateMap<Chapter, RespondChapterDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.VideoUrl))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
            .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(src => src.IsPublished))
            .ForMember(dest => dest.IsFree, opt => opt.MapFrom(src => src.IsFree))
            .ForMember(dest => dest.MuxDataId, opt => opt.MapFrom(src => src.MuxDataId))
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId));
    }
}