using Exceptions;
using Requests;
using Responses;
using MockDB;
using Mappers;

namespace Services;

public interface IAuthService
{
    AuthResponse SignIn(AuthRequest request);
}

public class AuthService : IAuthService
{
    private readonly IJwtService _jwtService;
    private readonly IHashingService _hashingService;
    private readonly IUserRepo _userRepository;

    private const string InvalidLoginMessage = "Login is invalid!";

    public AuthService(IUserRepo userRepo, IHashingService hashingService,
        IJwtService jwtService)
    {
        _jwtService = jwtService;
        _hashingService = hashingService;
        _userRepository = userRepo;
    }

    public AuthResponse SignIn(AuthRequest request)
    {
        var user = _userRepository.FindByEmail(request.Email!);

        if (user is null)
            throw new UnathorizedException(InvalidLoginMessage);

        var isPasswordValid = _hashingService.Verify(request.Password!, user.Password!);

        if (!isPasswordValid)
            throw new UnathorizedException(InvalidLoginMessage);

        var jwt = _jwtService.CreateToken(user);
        return new AuthResponse
        {
            Token = jwt,
            User = UserMapper.ToResponse(user),
            Cart = CartMapper.ToResponse(new CartService(UserMapper.ToResponse(user)));
        };
    }
}
