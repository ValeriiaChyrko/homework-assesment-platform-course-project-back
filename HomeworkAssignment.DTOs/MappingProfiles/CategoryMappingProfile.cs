using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs.CategoryRelated;
using HomeAssignment.DTOs.RespondDTOs.CategoryRelated;

namespace HomeAssignment.DTOs.MappingProfiles;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<RequestCategoryDto, Category>()
            .ConstructUsing(dto => Category.Create(dto.Name, null));

        CreateMap<Category, CategoryEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Courses, opt => opt.Ignore());

        CreateMap<CategoryEntity, Category>()
            .ConstructUsing(src => new Category(src.Id, src.Name,
                src.Courses != null ? src.Courses.Select(c => c.Id).ToList() : new List<Guid>()));

        CreateMap<Category, RespondCategoryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}