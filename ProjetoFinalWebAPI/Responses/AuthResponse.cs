﻿using System;

namespace Responses;

public class JwtResponse
{
    public string? Jwt { get; set; }
    public DateTime Expiration { get; set; }
}

public class AuthResponse
{
    public JwtResponse Token { get; set; }
}
