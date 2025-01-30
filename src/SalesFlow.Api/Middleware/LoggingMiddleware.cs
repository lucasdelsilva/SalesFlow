using System.Diagnostics;

namespace SalesFlow.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    private readonly DiagnosticListener _diagnostics;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _diagnostics = new DiagnosticListener("SalesFlow.Api");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var requestBody = await GetRequestBody(context.Request);


            _logger.LogInformation(
                "SalesFlow.Api (HTTP) | {RequestMethod} | {uri} | Iniciando Processo...",
                context.Request.Method,
                context.Request.Host + context.Request.Path
            );

            _logger.LogInformation(
                "SalesFlow.Api (HTTP) | {RequestMethod} | {uri} | Carregando...",
                context.Request.Method,
                context.Request.Host + context.Request.Path
            );

            _logger.LogInformation(
                "SalesFlow.Api (HTTP) | {RequestMethod} | {uri} | {RequestBody}",
                context.Request.Method,
                context.Request.Host + context.Request.Path,
                requestBody
            );

            await _next(context);

            sw.Stop();

            _logger.LogInformation(
                "SalesFlow.Api (HTTP) | {RequestMethod} | {uri} | Processo finalizado!",
                context.Request.Method,
                context.Request.Host + context.Request.Path
            );

            _logger.LogInformation(
                "SalesFlow.Api (HTTP) | {RequestMethod} | {uri} | Duração: {ElapsedMilliseconds}ms.",
                context.Request.Method,
                context.Request.Host + context.Request.Path,
                sw.ElapsedMilliseconds
            );

            _logger.LogInformation(
                "SalesFlow.Api (HTTP) | {RequestMethod} | {uri} | Status: {StatusCode}",
                context.Request.Method,
                context.Request.Host + context.Request.Path,
                context.Response.StatusCode
            );
        }
        catch (System.Exception ex)
        {
            sw.Stop();
            _logger.LogError(
                ex,
                "HTTP {RequestMethod} {uri} failed in {ElapsedMilliseconds}ms",
                context.Request.Method,
                context.Request.Host + context.Request.Path,
                sw.ElapsedMilliseconds
            );
            throw;
        }
    }

    private async Task<string> GetRequestBody(HttpRequest request)
    {
        if (!request.Body.CanSeek)
        {
            request.EnableBuffering();
        }

        request.Body.Position = 0;
        var reader = new StreamReader(request.Body);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        return body;
    }
}