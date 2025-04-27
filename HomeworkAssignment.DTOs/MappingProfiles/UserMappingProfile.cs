using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        MapDtoToDomain();
        MapDomainToEntity();
        MapDomainToRespondDto();
    }

    private void MapDtoToDomain()
    {
        CreateMap<UserDto, User>()
            .ConstructUsing(dto => User.CreateUser(
                dto.Id,
                dto.FullName,
                dto.Email,
                dto.GithubUsername,
                dto.GithubProfileUrl,
                dto.GithubPictureUrl
            ));
    }

    private void MapDomainToEntity()
    {
        CreateMap<User, UserEntity>()
            .ForMember(dest => dest.Attempts, opt => opt.Ignore())
            .ForMember(dest => dest.Courses, opt => opt.Ignore())
            .ForMember(dest => dest.Enrollments, opt => opt.Ignore())
            .ForMember(dest => dest.UsersProgress, opt => opt.Ignore());
    }

    private void MapDomainToRespondDto()
    {
        CreateMap<UserEntity, User>()
            .ConstructUsing(src => new User(
                src.UserRoles != null ? src.UserRoles.Select(a => a.RoleId).ToList() : null,
                src.Attempts != null ? src.Attempts.Select(a => a.Id).ToList() : null,
                src.Courses != null ? src.Courses.Select(c => c.Id).ToList() : null,
                src.Enrollments != null ? src.Enrollments.Select(e => e.Id).ToList() : null,
                src.UsersProgress != null ? src.UsersProgress.Select(up => up.Id).ToList() : null,
                src.Id,
                src.FullName,
                src.Email,
                src.GithubUsername,
                src.GithubProfileUrl,
                src.GithubPictureUrl,
                src.CreatedAt,
                src.UpdatedAt
            ));
    }
}