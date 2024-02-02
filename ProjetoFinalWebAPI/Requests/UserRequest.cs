namespace Requests;

public class BaseUserRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool Admin { get; set; }
}

public class ToUserResponse : BaseUserRequest
{
    public int Id { get; set; }
}
