using Responses;
using System;
using System.Text.Json;

namespace Exceptions
{
    public class DuplicateException : Exception, ICustomException
    {
        public DuplicateException(string message) : base(message)
        {
        }
        public int StatusCode { get => 409; }
        public string GetResponse() => JsonSerializer.Serialize(new ErrorResponse(base.Message));
    }
}


