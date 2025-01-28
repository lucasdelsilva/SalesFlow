using System.Net;

namespace SalesFlow.Exception.ExceptionBase;
public class InvalidLoginException : SalesFlowException
{
    public InvalidLoginException(string message) : base(message) { }

    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<object> GetErros()
    {
        return [Message];
    }
}
