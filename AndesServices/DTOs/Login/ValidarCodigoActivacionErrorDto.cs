using System;
using Newtonsoft.Json;
namespace AndesServices.DTOs.Login;

public class ValidarCodigoActivacionErrorDto
{
    [JsonProperty("error")]
    public string Error { get; set; } = string.Empty;
}
