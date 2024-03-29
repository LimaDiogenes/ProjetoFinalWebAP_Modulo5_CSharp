﻿using Responses;
using System;
using System.Text.Json;



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
