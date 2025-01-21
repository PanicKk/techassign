using System.Net;
using WMS.Models.Enums;

namespace WMS.Core.Exceptions;

public class WMSException : Exception
{
    public ExceptionType Type { get; set; } = ExceptionType.ServerError;
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
    public WMSException(string message, ExceptionType type, HttpStatusCode statusCode) : base(message)
    {
        Type = type;
        StatusCode = statusCode;
    }
}