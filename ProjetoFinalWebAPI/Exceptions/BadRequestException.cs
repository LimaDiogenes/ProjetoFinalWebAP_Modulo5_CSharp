using Responses;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Exceptions;

public class BadRequestException : Exception, ICustomException
{
    private List<ErrorMessageResponse> Errors { get; }

    public int StatusCode { get => 400; }

    public BadRequestException(List<ErrorMessageResponse> errors) : base()
    {
        Errors = errors;
    }

    public BadRequestException(string message) : base(message)
    {
        Errors = new List<ErrorMessageResponse>
        {
            new ErrorMessageResponse { Message = message }
        };
    }

    public string GetResponse() => JsonSerializer.Serialize(Errors);
}
