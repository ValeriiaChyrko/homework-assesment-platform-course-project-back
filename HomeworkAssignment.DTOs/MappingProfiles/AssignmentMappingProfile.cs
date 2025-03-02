using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class AssignmentMappingProfile : Profile
{
    public AssignmentMappingProfile()
    {
        CreateMap<RequestAssignmentDto, Assignment>()
            .ConstructUsing(dto => Assignment.Create(
                dto.Title,
                dto.Description,
                dto.RepositoryName,
                dto.RepositoryOwner,
                dto.RepositoryUrl,
                dto.Deadline,
                dto.MaxScore,
                dto.MaxAttemptsAmount,
                dto.Position,
                dto.IsPublished,
                dto.ChapterId,
                null, // Attempt IDs are not part of the DTO
                new ScoreSection(dto.AttemptCompilationSectionEnable, dto.AttemptCompilationMaxScore, dto.AttemptCompilationMinScore),
                new ScoreSection(dto.AttemptTestsSectionEnable, dto.AttemptTestsMaxScore, dto.AttemptTestsMinScore),
                new ScoreSection(dto.AttemptQualitySectionEnable, dto.AttemptQualityMaxScore, dto.AttemptQualityMinScore)
            ));
        
        CreateMap<Assignment, AssignmentEntity>()
            .ForMember(dest => dest.AttemptCompilationSectionEnable, opt => opt.MapFrom(src => src.CompilationSection.IsEnabled))
            .ForMember(dest => dest.AttemptCompilationMaxScore, opt => opt.MapFrom(src => src.CompilationSection.MaxScore))
            .ForMember(dest => dest.AttemptCompilationMinScore, opt => opt.MapFrom(src => src.CompilationSection.MinScore))

            .ForMember(dest => dest.AttemptTestsSectionEnable, opt => opt.MapFrom(src => src.TestsSection.IsEnabled))
            .ForMember(dest => dest.AttemptTestsMaxScore, opt => opt.MapFrom(src => src.TestsSection.MaxScore))
            .ForMember(dest => dest.AttemptTestsMinScore, opt => opt.MapFrom(src => src.TestsSection.MinScore))

            .ForMember(dest => dest.AttemptQualitySectionEnable, opt => opt.MapFrom(src => src.QualitySection.IsEnabled))
            .ForMember(dest => dest.AttemptQualityMaxScore, opt => opt.MapFrom(src => src.QualitySection.MaxScore))
            .ForMember(dest => dest.AttemptQualityMinScore, opt => opt.MapFrom(src => src.QualitySection.MinScore))

            .ForMember(dest => dest.Attempts, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) 
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt)); 
        
        CreateMap<AssignmentEntity, Assignment>()
            .ConstructUsing(entity => new Assignment(
                entity.Id,
                entity.Title,
                entity.Description,
                entity.RepositoryName,
                entity.RepositoryOwner,
                entity.RepositoryUrl,
                entity.Deadline,
                entity.MaxScore,
                entity.MaxAttemptsAmount,
                entity.Position,
                entity.IsPublished,
                entity.ChapterId,
                entity.Attempts != null ? entity.Attempts.Select(a => a.Id).ToList() : null,
                entity.CreatedAt,
                entity.UpdatedAt,
                new ScoreSection(entity.AttemptCompilationSectionEnable, entity.AttemptCompilationMaxScore, entity.AttemptCompilationMinScore),
                new ScoreSection(entity.AttemptTestsSectionEnable, entity.AttemptTestsMaxScore, entity.AttemptTestsMinScore),
                new ScoreSection(entity.AttemptQualitySectionEnable, entity.AttemptQualityMaxScore, entity.AttemptQualityMinScore)
            ))
            .ForMember(dest => dest.AttemptProgressIds, opt => opt.MapFrom(src => src.Attempts != null ? src.Attempts.Select(a => a.Id).ToList() : new List<Guid>()));
        
        CreateMap<Assignment, RespondAssignmentDto>()
            .ForMember(dest => dest.RepositoryOwner, opt => opt.MapFrom(src => src.RepositoryOwnerUserName))
            .ForMember(dest => dest.AttemptCompilationSectionEnable, opt => opt.MapFrom(src => src.CompilationSection.IsEnabled))
            .ForMember(dest => dest.AttemptTestsSectionEnable, opt => opt.MapFrom(src => src.TestsSection.IsEnabled))
            .ForMember(dest => dest.AttemptQualitySectionEnable, opt => opt.MapFrom(src => src.QualitySection.IsEnabled))
            .ForMember(dest => dest.AttemptCompilationMaxScore, opt => opt.MapFrom(src => src.CompilationSection.MaxScore))
            .ForMember(dest => dest.AttemptCompilationMinScore, opt => opt.MapFrom(src => src.CompilationSection.MinScore))
            .ForMember(dest => dest.AttemptTestsMaxScore, opt => opt.MapFrom(src => src.TestsSection.MaxScore))
            .ForMember(dest => dest.AttemptTestsMinScore, opt => opt.MapFrom(src => src.TestsSection.MinScore))
            .ForMember(dest => dest.AttemptQualityMaxScore, opt => opt.MapFrom(src => src.QualitySection.MaxScore))
            .ForMember(dest => dest.AttemptQualityMinScore, opt => opt.MapFrom(src => src.QualitySection.MinScore));
    }
}