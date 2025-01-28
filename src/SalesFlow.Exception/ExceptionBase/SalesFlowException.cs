namespace SalesFlow.Exception.ExceptionBase;
public abstract class SalesFlowException : SystemException
{
    protected SalesFlowException(string message) : base(message) { }

    public abstract int StatusCode { get; }
    public abstract List<Object> GetErros();
}