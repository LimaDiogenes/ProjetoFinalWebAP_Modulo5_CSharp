using Responses;
using System;
using System.Text.Json;

namespace Exceptions;

public class UnathorizedException : Exception, ICustomException
{
    public UnathorizedException(string message) : base(message)
    {
    }
    public int StatusCode { get => 401; }

    public string GetResponse() => JsonSerializer.Serialize(new ErrorResponse(base.Message));
}
