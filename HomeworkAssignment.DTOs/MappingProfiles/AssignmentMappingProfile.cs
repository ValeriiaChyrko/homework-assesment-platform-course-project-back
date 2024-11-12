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
        CreateMap<Assignment, RespondAssignmentDto>();

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
            }))
            .ReverseMap()
            .ForPath(src => src.AttemptCompilationSectionEnable,
                opt => opt.MapFrom(dest => dest.CompilationSection.IsEnabled))
            .ForPath(src => src.AttemptCompilationMaxScore,
                opt => opt.MapFrom(dest => dest.CompilationSection.MaxScore))
            .ForPath(src => src.AttemptCompilationMinScore,
                opt => opt.MapFrom(dest => dest.CompilationSection.MinScore))
            .ForPath(src => src.AttemptTestsSectionEnable, opt => opt.MapFrom(dest => dest.TestsSection.IsEnabled))
            .ForPath(src => src.AttemptTestsMaxScore, opt => opt.MapFrom(dest => dest.TestsSection.MaxScore))
            .ForPath(src => src.AttemptTestsMinScore, opt => opt.MapFrom(dest => dest.TestsSection.MinScore))
            .ForPath(src => src.AttemptQualitySectionEnable, opt => opt.MapFrom(dest => dest.QualitySection.IsEnabled))
            .ForPath(src => src.AttemptQualityMaxScore, opt => opt.MapFrom(dest => dest.QualitySection.MaxScore))
            .ForPath(src => src.AttemptQualityMinScore, opt => opt.MapFrom(dest => dest.QualitySection.MinScore));
    }
}