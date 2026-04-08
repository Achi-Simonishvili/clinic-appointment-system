using System.Net;

namespace ClinicSystem.Application.Common;

public class CommonResponse
{
    public string Message { get; set; } = string.Empty;
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public object? Result { get; set; }
}
