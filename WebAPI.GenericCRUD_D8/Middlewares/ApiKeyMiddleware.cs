namespace WebAPI.GenericCRUD_D8.Middlewares
{
    public class ApiKeyMiddleware
    {
        private const string API_KEY_HEADER_NAME = "X-Api-Key";
        private readonly RequestDelegate _next;
        private readonly string _validApiKey;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _validApiKey = configuration["TenantInfo:APIKey"];
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var receivedApiKey) || receivedApiKey != _validApiKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid or missing API Key.");
                return;
            }

            await _next(context);
        }
    }
}
