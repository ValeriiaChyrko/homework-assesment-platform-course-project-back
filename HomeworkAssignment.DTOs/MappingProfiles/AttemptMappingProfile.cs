using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class AttemptMappingProfile : Profile
{
    public AttemptMappingProfile()
    {
        CreateMap<Attempt, AttemptEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Student, opt => opt.Ignore())
            .ForMember(dest => dest.Assignment, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Attempt, RespondAttemptDto>()
            .ReverseMap();
    }
}