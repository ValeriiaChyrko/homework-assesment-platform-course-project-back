using AutoMapper;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class TeacherMappingProfile : Profile
{
    public TeacherMappingProfile()
    {
        CreateMap<Teacher, RespondTeacherDto>()
            .ReverseMap();
    }
}