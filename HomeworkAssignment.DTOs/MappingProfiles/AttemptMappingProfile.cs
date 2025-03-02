using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class AttemptMappingProfile : Profile
{
    public AttemptMappingProfile()
    {
        CreateMap<RequestAttemptDto, Attempt>()
            .ConstructUsing(dto => Attempt.Create(
                dto.Position,
                dto.BranchName,
                dto.FinalScore,
                dto.CompilationScore,
                dto.QualityScore,
                dto.TestsScore,
                dto.IsCompleted,
                dto.UserId,
                dto.AssignmentId
            ))
            .ForMember(dest => dest.ProgressStatus, opt => opt.MapFrom(src => src.ProgressStatus.ToLower()))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        
        CreateMap<Attempt, AttemptProgressEntity>()
            .ForMember(dest => dest.User, opt => opt.Ignore()) 
            .ForMember(dest => dest.Assignment, opt => opt.Ignore()) 
            .ReverseMap();
        
        CreateMap<Attempt, RespondAttemptDto>();
    }
}