using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class MuxDataMappingProfile : Profile
{
    public MuxDataMappingProfile()
    {
        CreateMap<RequestMuxDataDto, MuxData>()
            .ConstructUsing(dto => MuxData.Create(
                dto.AssetId,
                dto.PlaybackId,
                dto.ChapterId
            ));
        
        CreateMap<MuxData, MuxDataEntity>()
            .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId));

        CreateMap<MuxDataEntity, MuxData>()
            .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId));
        
        CreateMap<MuxData, RespondMuxDataDto>();
    }
}