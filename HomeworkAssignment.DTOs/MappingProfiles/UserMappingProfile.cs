using AutoMapper;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.UserRelated;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeAssignment.DTOs.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<RequestUserDto, User>()
            .ConstructUsing(dto => User.CreateStudent(
                dto.FullName,
                dto.Email,
                dto.Password, // TODO: Тут має бути хешування пароля перед створенням User
                dto.GithubUsername,
                dto.GithubProfileUrl,
                dto.GithubPictureUrl
            ));
        
        CreateMap<User, UserEntity>()
            .ForMember(dest => dest.Attempts, opt => opt.Ignore())
            .ForMember(dest => dest.Courses, opt => opt.Ignore())
            .ForMember(dest => dest.Enrollments, opt => opt.Ignore())
            .ForMember(dest => dest.UsersProgress, opt => opt.Ignore());

        CreateMap<UserEntity, User>()
            .ConstructUsing(src => new User(
                src.Attempts != null ? src.Attempts.Select(a => a.Id).ToList() : null,
                src.Courses != null ? src.Courses.Select(c => c.Id).ToList() : null,
                src.Enrollments != null ? src.Enrollments.Select(e => e.Id).ToList() : null,
                src.UsersProgress != null ? src.UsersProgress.Select(up => up.Id).ToList() : null,
                src.Id,
                src.FullName,
                src.Email,
                src.PasswordHash,
                src.RoleType,
                src.GithubUsername,
                src.GithubProfileUrl,
                src.GithubPictureUrl,
                src.CreatedAt,
                src.UpdatedAt
            ));
    }
}