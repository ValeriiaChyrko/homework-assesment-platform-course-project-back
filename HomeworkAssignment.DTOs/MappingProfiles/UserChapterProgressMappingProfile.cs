using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class UserChapterProgressMappingProfile : Profile
{
    public UserChapterProgressMappingProfile()
    {
        CreateMap<ChapterUserProgress, UserChapterProgressEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.User, opt => opt.Ignore()) 
            .ForMember(dest => dest.Chapter, opt => opt.Ignore()); 

        CreateMap<UserChapterProgressEntity, ChapterUserProgress>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        
        CreateMap<ChapterUserProgress, RespondChapterUserProgressDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId));
    }
}