using System.Globalization;

namespace Standard_Solution.API.Exceptions
{
    public class ForbiddenException : Exception
    {
        public int StatusCode { get; }
        public object Value { get; }

        public ForbiddenException() : base() { }

        public ForbiddenException(string message) : base(message) { }

        public ForbiddenException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
    }
}