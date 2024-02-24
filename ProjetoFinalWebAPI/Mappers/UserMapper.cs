using Entities;
using Requests;

namespace Mappers;

public static class UserMapper
{
    public static UserResponse ToResponse(User user) => new UserResponse
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Admin = user.Admin
        //UserCart = CartMapper.ToResponse(user.UserCart)
    };

    public static User ToEntity(BaseUserRequest user) => new User(
        name: user.Name!,
        email: user.Email!,
        admin: user.Admin,
        password: user.Password!
    );

    public static User ToEntity(ToUserResponse user) => new User(
        name: user.Name!,
        email: user.Email!,
        admin: user.Admin,
        password: user.Password!
    );
}
