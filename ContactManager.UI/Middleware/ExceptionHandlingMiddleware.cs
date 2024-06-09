using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Threading.Tasks;

namespace CRUDExample.Middleware
{
	// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
	public class ExceptionHandlingMiddleware
	{
		private readonly ILogger<ExceptionHandlingMiddleware> logger;
		private readonly RequestDelegate _next;

		private readonly IDiagnosticContext diagnosticContext;
		public ExceptionHandlingMiddleware(IDiagnosticContext diagnosticContext, ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
		{
			this.diagnosticContext = diagnosticContext;
			this.logger = logger;

			_next = next;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					logger.LogError("{ExceptionType}  {ExceptionMessage} ", ex.InnerException.GetType().ToString(), ex.Message);

				}
				else
				{
					logger.LogError("{ExceptionType}  {ExceptionMessage} ", ex.GetType().ToString(), ex.Message);
				}
				//httpContext.Response.StatusCode = 500;
				//await httpContext.Response.WriteAsync(ex.Message);


				throw;
				//await httpContext.Response.WriteAsync("ERROR Occured");
			}
		}
	}

	// Extension method used to add the middleware to the HTTP request pipeline.
	public static class ExceptionHandlingMiddlewareExtensions
	{
		public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ExceptionHandlingMiddleware>();
		}
	}
}
