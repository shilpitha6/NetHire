using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetHire.DTO.Response;

public partial class ApiResponse
{
    //[JsonProperty("responseStatus")]
    public Boolean ResponseSuccess { get; set; }
    //[JsonProperty("status")]
    public int Status { get; set; }
    //[JsonProperty("id")]
    public int Id { get; set; }
    //[JsonProperty("message")]
    public string? Message { get; set; }
    //[JsonProperty("data")]
    public object? Data { get; set; }
}