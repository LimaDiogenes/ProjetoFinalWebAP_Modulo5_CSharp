using Responses;
using System;
using System.Collections.Generic;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Exceptions
{
    public class AdminOpsException : Exception, ICustomException
    {
        public AdminOpsException(string message) : base(message)
        {
        }
        public int StatusCode { get => 503; }

        public string GetResponse() => JsonSerializer.Serialize(new ErrorResponse(base.Message));
    }

}
