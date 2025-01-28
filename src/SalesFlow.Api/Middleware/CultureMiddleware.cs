using System.Globalization;

namespace SalesFlow.Api.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;
    public CultureMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        var cultureInfo = new CultureInfo("en");

        var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
        var requestCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();


        if (!string.IsNullOrWhiteSpace(requestCulture) && supportedLanguages.Exists(lang => lang.Name.Equals(requestCulture)))
            cultureInfo = new CultureInfo(requestCulture);

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}