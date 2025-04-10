using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.AttemptRelated;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.RespondDTOs.AttemptRelated;

namespace HomeAssignment.DTOs.MappingProfiles;

public class AttemptMappingProfile : Profile
{
    public AttemptMappingProfile()
    {
        CreateMap<RequestAttemptDto, Attempt>()
            .ConstructUsing(dto => Attempt.Create(
                dto.Position,
                dto.BranchName,
                dto.UserId,
                dto.AssignmentId
            ))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        
        CreateMap<Attempt, AttemptEntity>()
            .ForMember(dest => dest.User, opt => opt.Ignore()) 
            .ForMember(dest => dest.Assignment, opt => opt.Ignore()) 
            .ReverseMap();
        
        CreateMap<Attempt, RespondAttemptDto>();
        
        CreateMap<RequestSubmitAttemptDto, RequestRepositoryWithBranchDto>()
            .ForMember(dest => dest.RepoTitle, opt => opt.MapFrom(src => src.Assignment.RepositoryName))
            .ForMember(dest => dest.BranchTitle, opt => opt.MapFrom(src => src.Attempt.BranchName))
            .ForMember(dest => dest.OwnerGitHubUsername, opt => opt.MapFrom(src => src.Assignment.RepositoryOwner))
            .ForMember(dest => dest.AuthorGitHubUsername, opt => opt.MapFrom(src => src.AuthorGitHubUsername));
    }
}