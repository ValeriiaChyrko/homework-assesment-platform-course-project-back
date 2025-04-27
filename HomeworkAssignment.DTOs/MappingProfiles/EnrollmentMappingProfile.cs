using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs.CourseRelated;
using HomeAssignment.DTOs.RespondDTOs.CourseRelated;

namespace HomeAssignment.DTOs.MappingProfiles;

public class EnrollmentMappingProfile : Profile
{
    public EnrollmentMappingProfile()
    {
        MapRequestToDomain();
        MapEntityToDomain();
        MapDomainToEntity();
        MapDomainToRespondDto();
    }

    private void MapRequestToDomain()
    {
        CreateMap<RequestEnrollmentDto, Enrollment>()
            .ConstructUsing(dto => Enrollment.Create(dto.UserId, dto.CourseId));
    }

    private void MapEntityToDomain()
    {
        CreateMap<EnrollmentEntity, Enrollment>()
            .ConstructUsing(entity => new Enrollment(
                entity.Id,
                entity.UserId,
                entity.CourseId,
                entity.CreatedAt,
                entity.UpdatedAt
            ));
    }

    private void MapDomainToEntity()
    {
        CreateMap<Enrollment, EnrollmentEntity>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
    }

    private void MapDomainToRespondDto()
    {
        CreateMap<Enrollment, RespondEnrollmentDto>();
    }
}