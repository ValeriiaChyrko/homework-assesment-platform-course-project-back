using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class AssignmentMappingProfile : Profile
{
    public AssignmentMappingProfile()
    {
        CreateMap<Assignment, RespondAssignmentDto>()
            .ForMember(dest => dest.CompilationSection, opt => opt.MapFrom(src => new ScoreSectionDto
            {
                IsEnabled = src.CompilationSection.IsEnabled,
                MaxScore = src.CompilationSection.MaxScore,
                MinScore = src.CompilationSection.MinScore
            }))
            .ForMember(dest => dest.TestsSection, opt => opt.MapFrom(src => new ScoreSectionDto
            {
                IsEnabled = src.TestsSection.IsEnabled,
                MaxScore = src.TestsSection.MaxScore,
                MinScore = src.TestsSection.MinScore
            }))
            .ForMember(dest => dest.QualitySection, opt => opt.MapFrom(src => new ScoreSectionDto
            {
                IsEnabled = src.QualitySection.IsEnabled,
                MaxScore = src.QualitySection.MaxScore,
                MinScore = src.QualitySection.MinScore
            }));

        CreateMap<AssignmentEntity, RespondAssignmentDto>()
            .ForMember(dest => dest.CompilationSection, opt => opt.MapFrom(src => new ScoreSectionDto
            {
                IsEnabled = src.AttemptCompilationSectionEnable,
                MaxScore = src.AttemptCompilationMaxScore,
                MinScore = src.AttemptCompilationMinScore
            }))
            .ForMember(dest => dest.TestsSection, opt => opt.MapFrom(src => new ScoreSectionDto
            {
                IsEnabled = src.AttemptTestsSectionEnable,
                MaxScore = src.AttemptTestsMaxScore,
                MinScore = src.AttemptTestsMinScore
            }))
            .ForMember(dest => dest.QualitySection, opt => opt.MapFrom(src => new ScoreSectionDto
            {
                IsEnabled = src.AttemptQualitySectionEnable,
                MaxScore = src.AttemptQualityMaxScore,
                MinScore = src.AttemptQualityMinScore
            }));

        CreateMap<AssignmentEntity, Assignment>()
            .ConstructUsing(src => new Assignment(
                src.Id,
                src.OwnerId,
                src.Title,
                src.Description,
                src.RepositoryName,
                src.Deadline,
                src.MaxScore,
                src.MaxAttemptsAmount,
                new ScoreSection(src.AttemptCompilationSectionEnable, src.AttemptCompilationMaxScore,
                    src.AttemptCompilationMinScore),
                new ScoreSection(src.AttemptTestsSectionEnable, src.AttemptTestsMaxScore, src.AttemptTestsMinScore),
                new ScoreSection(src.AttemptQualitySectionEnable, src.AttemptQualityMaxScore,
                    src.AttemptQualityMinScore)
            ))
            .ReverseMap()
            .ForMember(dest => dest.AttemptCompilationSectionEnable,
                opt => opt.MapFrom(src => src.CompilationSection.IsEnabled))
            .ForMember(dest => dest.AttemptCompilationMaxScore,
                opt => opt.MapFrom(src => src.CompilationSection.MaxScore))
            .ForMember(dest => dest.AttemptCompilationMinScore,
                opt => opt.MapFrom(src => src.CompilationSection.MinScore))
            .ForMember(dest => dest.AttemptTestsSectionEnable, opt => opt.MapFrom(src => src.TestsSection.IsEnabled))
            .ForMember(dest => dest.AttemptTestsMaxScore, opt => opt.MapFrom(src => src.TestsSection.MaxScore))
            .ForMember(dest => dest.AttemptTestsMinScore, opt => opt.MapFrom(src => src.TestsSection.MinScore))
            .ForMember(dest => dest.AttemptQualitySectionEnable,
                opt => opt.MapFrom(src => src.QualitySection.IsEnabled))
            .ForMember(dest => dest.AttemptQualityMaxScore, opt => opt.MapFrom(src => src.QualitySection.MaxScore))
            .ForMember(dest => dest.AttemptQualityMinScore, opt => opt.MapFrom(src => src.QualitySection.MinScore));
    }
}