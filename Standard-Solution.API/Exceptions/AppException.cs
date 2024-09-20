using System.Globalization;

namespace Standard_Solution.API.Exceptions;

public class AppException : Exception
{
    public int StatusCode { get; }
    public object Value { get; }
    public AppException(string message) : base(message) { }
    public AppException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
}
