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
        MapRequestToDomain();
        MapDomainToEntity();
        MapEntityToDomain();
        MapDomainToRespondDto();
    }

    private void MapRequestToDomain()
    {
        CreateMap<RequestCategoryDto, Category>()
            .ConstructUsing(dto => Category.Create(dto.Name, null));
    }

    private void MapDomainToEntity()
    {
        CreateMap<Category, CategoryEntity>()
            .ForMember(dest => dest.Courses, opt => opt.Ignore());
    }

    private void MapEntityToDomain()
    {
        CreateMap<CategoryEntity, Category>()
            .ConstructUsing(src => new Category(
                src.Id,
                src.Name, 
                src.Courses != null ? src.Courses.Select(a => a.Id).ToList() : null
            ));
    }

    private void MapDomainToRespondDto()
    {
        CreateMap<Category, RespondCategoryDto>();
    }
}