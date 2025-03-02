using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class UserProgressMappingProfile : Profile
{
    public UserProgressMappingProfile()
    {
        CreateMap<RequestUserProgressDto, UserProgress>()
            .ConstructUsing(dto => UserProgress.Create(
                dto.IsCompleted,
                dto.UserId,
                dto.ChapterId
            ));
        
        CreateMap<UserProgress, UserProgressEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.User, opt => opt.Ignore()) 
            .ForMember(dest => dest.Chapter, opt => opt.Ignore()); 

        CreateMap<UserProgressEntity, UserProgress>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        
        CreateMap<UserProgress, RespondUserProgressDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId));
    }
}