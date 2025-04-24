using HomeAssignment.DTOs.SharedDTOs;

namespace HomeworkAssignment.Application.Abstractions.UserRelated;

public interface IUserService
{
    Task CreateOrUpdateUserAcync(UserDto userDto, CancellationToken cancellationToken = default);
}