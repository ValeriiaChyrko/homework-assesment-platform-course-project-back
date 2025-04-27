using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs.AttemptRelated;
using HomeAssignment.DTOs.RespondDTOs.AttemptRelated;

namespace HomeAssignment.DTOs.MappingProfiles;

public class AttemptMappingProfile : Profile
{
    public AttemptMappingProfile()
    {
        MapDomainToEntity();
        MapEntityToDomain();
        MapDomainToDto();
        MapSubmitDtoToRepoRequest();
    }

    private void MapDomainToEntity()
    {
        CreateMap<Attempt, AttemptEntity>()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Assignment, opt => opt.Ignore());
    }

    private void MapEntityToDomain()
    {
        CreateMap<AttemptEntity, Attempt>();
    }

    private void MapDomainToDto()
    {
        CreateMap<Attempt, RespondAttemptDto>();
    }

    private void MapSubmitDtoToRepoRequest()
    {
        CreateMap<RequestSubmitAttemptDto, RequestRepositoryWithBranchDto>()
            .ForMember(dest => dest.RepoTitle, opt => opt.MapFrom(src => src.Assignment.RepositoryName))
            .ForMember(dest => dest.BranchTitle, opt => opt.MapFrom(src => src.Attempt.BranchName))
            .ForMember(dest => dest.OwnerGitHubUsername, opt => opt.MapFrom(src => src.Assignment.RepositoryOwner))
            .ForMember(dest => dest.AuthorGitHubUsername, opt => opt.MapFrom(src => src.AuthorGitHubUsername));
    }
}