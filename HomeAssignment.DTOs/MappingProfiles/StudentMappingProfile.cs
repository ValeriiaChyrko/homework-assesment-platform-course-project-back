using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class StudentMappingProfile : Profile
{
    public StudentMappingProfile()
    {
        CreateMap<Student, RespondStudentDto>()
            .ReverseMap();
    }
}