using Exceptions;
using Requests;
using Validators;
using MockDB;
using Mappers;
using System.Collections.Generic;
using System.Linq;
using Entities;
using System.Security.Claims;

namespace Services;

public interface IUserService
{
    List<UserResponse> ListUsers();
    UserResponse? GetUserById(int id);
    UserResponse CreateUser(BaseUserRequest newUser);
    UserResponse UpdateUser(ToUserResponse updatedUser);
    bool DeleteUser(int id);
}

public class UserService : IUserService
{    
    private readonly IValidator<BaseUserRequest> _validator;
    private readonly IUserRepo _repository;
    private readonly IHashingService _hashingService;
    

    public UserService(IUserRepo repository, IValidator<BaseUserRequest> validator,
        IHashingService hashingService)
    {
        
        _validator = validator;
        _repository = repository;
        _hashingService = hashingService;
    }

    public List<UserResponse> ListUsers()
    {
        var users = _repository.ListUsers();
        var response = users.Select(user => UserMapper.ToResponse(user)).ToList();
        return response;
    }

    public UserResponse? GetUserById(int id)
    {
        var user = _repository.GetById(id);
        return user is null ? null : UserMapper.ToResponse(user);
    }

    public UserResponse CreateUser(BaseUserRequest request)
    {
        
        var errors = _validator.Validate(request);
        

        if (errors.Any())
            throw new BadRequestException(errors);

        var newUser = UserMapper.ToEntity(request);
        newUser.Password = _hashingService.Hash(newUser.Password!);

        var user = _repository.CreateUser(newUser);
        return UserMapper.ToResponse(user);
    }

    public UserResponse UpdateUser(ToUserResponse request)
    {
        var errors = _validator.Validate(request);

        if (errors.Any())
            throw new BadRequestException(errors);

        var existingUser = _repository.GetById(request.Id);

        if (existingUser is null)
            throw new NotFoundException("User not found!");

        var updateUser = UserMapper.ToEntity(request);
        updateUser.Password = _hashingService.Hash(updateUser.Password!);

        var user = _repository.UpdateUser(updateUser);
        return UserMapper.ToResponse(user);
    }

    public bool DeleteUser(int id)
    {
        return _repository.DeleteUser(id);
    }
}
