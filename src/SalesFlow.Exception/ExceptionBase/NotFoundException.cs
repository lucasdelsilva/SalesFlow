using System.Net;

namespace SalesFlow.Exception.ExceptionBase;
public class NotFoundException : SalesFlowException
{
    public NotFoundException(string message) : base(message) { }
    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<object> GetErros()
    {
        return [Message];
    }
}