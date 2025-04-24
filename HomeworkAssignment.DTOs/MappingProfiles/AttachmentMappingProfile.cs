using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs.AttachmentRelated;

namespace HomeAssignment.DTOs.MappingProfiles;

public class AttachmentMappingProfile : Profile
{
    public AttachmentMappingProfile()
    {
        MapDomainToEntity();
        MapEntityToDomain();
        MapDomainToDto();
    }

    private void MapDomainToEntity()
    {
        CreateMap<Attachment, AttachmentEntity>();
    }

    private void MapEntityToDomain()
    {
        CreateMap<AttachmentEntity, Attachment>();
    }

    private void MapDomainToDto()
    {
        CreateMap<Attachment, RespondAttachmentDto>()
            .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.UploadthingKey));
    }
}