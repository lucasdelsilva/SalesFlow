using System.Net;

namespace SalesFlow.Exception.ExceptionBase;
public class ErrorOnValidationException : SalesFlowException
{
    private List<object> Erros { get; set; }
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public ErrorOnValidationException(List<object> errorMessages) : base(string.Empty) => Erros = errorMessages;

    public override List<object> GetErros()
    {
        return Erros;
    }
}