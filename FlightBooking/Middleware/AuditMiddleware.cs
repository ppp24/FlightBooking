using FlightBooking.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlightBooking.Middleware
{
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditMiddleware> _logger;
        private readonly IServiceProvider _serviceProvider;

        public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger, IServiceProvider serviceProvider)
        {
            _next = next;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.User.Identity.IsAuthenticated ? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value : null;
            var userName = context.User.Identity.IsAuthenticated ? context.User.Identity.Name : "Anonymous";
            var action = $"{context.Request.Method} {context.Request.Path}";
            var details = $"QueryString: {context.Request.QueryString}";

            if (userId != null) // Only log if the user is authenticated
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var auditLog = new TblAuditLog
                    {
                        UserId = userId,
                        UserName = userName,
                        Action = action,
                        Timestamp = DateTime.UtcNow,
                        Details = details
                    };

                    dbContext.TblAuditLog.Add(auditLog);
                    await dbContext.SaveChangesAsync();
                }

                _logger.LogInformation($"UserId: {userId}, UserName: {userName}, Action: {action}, Details: {details}");
            }
            else
            {
                _logger.LogInformation($"Anonymous user performed action: {action}, Details: {details}");
            }

            await _next(context);
        }


    }
}
