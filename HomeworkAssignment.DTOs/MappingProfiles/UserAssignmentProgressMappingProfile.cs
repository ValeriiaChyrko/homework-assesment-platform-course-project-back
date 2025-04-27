using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs.AssignmentRelated;

namespace HomeAssignment.DTOs.MappingProfiles;

public class UserAssignmentProgressMappingProfile : Profile
{
    public UserAssignmentProgressMappingProfile()
    {
        MapEntityToDomain();
        MapDomainToEntity();
        MapDomainToRespondDto();
    }

    private void MapEntityToDomain()
    {
        CreateMap<UserAssignmentProgressEntity, AssignmentUserProgress>();
    }

    private void MapDomainToEntity()
    {
        CreateMap<AssignmentUserProgress, UserAssignmentProgressEntity>()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Assignment, opt => opt.Ignore());  
    }

    private void MapDomainToRespondDto()
    {
        CreateMap<AssignmentUserProgress, RespondAssignmentUserProgressDto>();
    }
}