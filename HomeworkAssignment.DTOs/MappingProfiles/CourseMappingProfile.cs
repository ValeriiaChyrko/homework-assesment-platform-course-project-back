using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs.CourseRelated;
using HomeAssignment.DTOs.RespondDTOs.CourseRelated;

namespace HomeAssignment.DTOs.MappingProfiles;

public class CourseMappingProfile : Profile
{
    public CourseMappingProfile()
    {
        MapCreateRequestToDomain();
        MapDomainToEntity();
        MapEntityToDomain();
        MapEntityToDetailView();
        MapDetailViewToDto();
        MapDomainToRespondDto();
    }

    private void MapCreateRequestToDomain()
    {
        CreateMap<RequestCreateCourseDto, Course>()
            .ConstructUsing(src => Course.Create(src.Title));
    }

    private void MapDomainToEntity()
    {
        CreateMap<Course, CourseEntity>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore())
            .ForMember(dest => dest.Enrollments, opt => opt.Ignore())
            .ForMember(dest => dest.Chapters, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());
    }

    private void MapEntityToDomain()
    {
        CreateMap<CourseEntity, Course>()
            .ConstructUsing(src => new Course(
                src.Id,
                src.Title,
                src.Description,
                src.ImageUrl,
                src.IsPublished,
                src.UserId,
                src.CategoryId,
                src.Attachments != null ? src.Attachments.Select(a => a.Id).ToList() : null,
                src.CreatedAt,
                src.UpdatedAt
            ));
    }

    private void MapEntityToDetailView()
    {
        CreateMap<CourseEntity, CourseDetailView>()
            .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments))
            .ForMember(dest => dest.Chapters, opt => opt.MapFrom(src => src.Chapters))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
    }

    private void MapDetailViewToDto()
    {
        CreateMap<CourseDetailView, RespondCourseFullInfoDto>();
    }

    private void MapDomainToRespondDto()
    {
        CreateMap<Course, RespondCourseDto>();
    }
}