namespace SalesFlow.Communication.Response;
public class ResponseErrorJson
{
    public List<Object> Erros { get; set; } = [];

    public ResponseErrorJson(List<Object> modelErros)
    {
        Erros = modelErros;
    }

    public ResponseErrorJson(string modelErro)
    {
        Erros = [modelErro];
    }
}