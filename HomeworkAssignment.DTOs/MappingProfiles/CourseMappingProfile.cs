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
        CreateMap<RequestCreateCourseDto, Course>()
            .ConstructUsing(src => Course.Create(
                src.Title
            ));

        CreateMap<Course, CourseEntity>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore())
            .ForMember(dest => dest.Enrollments, opt => opt.Ignore())
            .ForMember(dest => dest.Chapters, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

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

        CreateMap<CourseEntity, CourseDetailView>()
            .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments))
            .ForMember(dest => dest.Chapters, opt => opt.MapFrom(src => src.Chapters))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

        CreateMap<CourseDetailView, RespondCourseFullInfoDto>();

        CreateMap<Course, RespondCourseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(src => src.IsPublished))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
    }
}