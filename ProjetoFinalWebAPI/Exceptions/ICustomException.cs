﻿namespace Exceptions;

public interface ICustomException
{
    public int StatusCode { get; }
    public string GetResponse();
}
