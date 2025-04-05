using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class UserAssignmentProgressMappingProfile : Profile
{
    public UserAssignmentProgressMappingProfile()
    {
        CreateMap<RequestAssignmentUserProgressDto, AssignmentUserProgress>()
            .ConstructUsing(dto => AssignmentUserProgress.Create(
                dto.IsCompleted,
                dto.UserId,
                dto.AssignmentId
            ));
        
        CreateMap<AssignmentUserProgress, UserAssignmentProgressEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.AssignmentId, opt => opt.MapFrom(src => src.AssignmentId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.User, opt => opt.Ignore()) 
            .ForMember(dest => dest.Assignment, opt => opt.Ignore()); 

        CreateMap<UserAssignmentProgressEntity, AssignmentUserProgress>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.AssignmentId, opt => opt.MapFrom(src => src.AssignmentId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        
        CreateMap<AssignmentUserProgress, RespondAssignmentUserProgressDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.AssignmentId, opt => opt.MapFrom(src => src.AssignmentId));
    }
}