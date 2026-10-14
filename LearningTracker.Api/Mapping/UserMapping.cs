using LearningTracker.Api.Dto;
using LearningTracker.Api.Models;

namespace LearningTracker.Api.Mapping;

public static class UserMapping
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
        };
    }
}
