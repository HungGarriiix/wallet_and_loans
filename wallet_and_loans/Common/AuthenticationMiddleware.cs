namespace wallet_and_loans_api.Common
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public AuthenticationMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using var scope = _serviceProvider.CreateScope();

            var 
            // Check if the request has the "Authorization" header
            //if (!context.Request.Headers.ContainsKey("Authorization"))
            //{
            //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //    await context.Response.WriteAsync("Unauthorized: Missing Authorization header.");
            //    return;
            //}

            //string token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Validate the token (this is a placeholder, implement your own validation logic)
            if (!context.Request.Headers.TryGetValue("X-Session-Id", out var userId) 
                && !context.Request.Headers.TryGetValue("X-Platform-Id", out var platform))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid token.");
                return;
            }
            else
            {
                // Optionally, you can add the user ID to the HttpContext for later use in controllers
                context.Items["UserId"] = userId.ToString();
            }

            // If the token is valid, proceed to the next middleware
            await _next(context);
        }

        //private bool ValidateToken(string token)
        //{
        //    // Implement your token validation logic here
        //    // For example, you could check if the token exists in a database or if it's a valid JWT
        //    return !string.IsNullOrEmpty(token); // Placeholder validation
        //}
    }
}
