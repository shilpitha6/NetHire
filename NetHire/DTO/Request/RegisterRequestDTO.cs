using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetHire.DTO.Request;

public partial class RegisterRequestDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
}