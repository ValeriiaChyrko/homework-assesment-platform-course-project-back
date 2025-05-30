﻿using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs.AssignmentRelated;
using HomeAssignment.DTOs.RespondDTOs.AssignmentRelated;

namespace HomeAssignment.DTOs.MappingProfiles;

public class AssignmentMappingProfile : Profile
{
    public AssignmentMappingProfile()
    {
        MapDtoToDomain();
        MapDomainToEntity();
        MapEntityToDomain();
        MapDomainToDto();
    }

    private void MapDtoToDomain()
    {
        CreateMap<RequestCreateAssignmentDto, Assignment>()
            .ConstructUsing(dto => Assignment.Create(dto.Title));
    }

    private void MapDomainToEntity()
    {
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
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }

    private void MapEntityToDomain()
    {
        CreateMap<AssignmentEntity, Assignment>()
            .ConvertUsing(entity => new Assignment(
                entity.Id,
                entity.Title,
                entity.Description,
                entity.RepositoryName,
                entity.RepositoryBaseBranchName,
                entity.RepositoryOwner,
                entity.RepositoryUrl,
                entity.Deadline ?? entity.CreatedAt,
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
            ));
    }

    private void MapDomainToDto()
    {
        CreateMap<Assignment, RespondAssignmentDto>()
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