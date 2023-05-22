using Api.Extensions;
using KissLog.AspNetCore;
using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.RequestLogsListener;
using KissLog.Formatters;

namespace Api.Configuration;

public static class LoggerConfig
{
    public static void AddLoggingConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(provider =>
        {
            provider
                .AddKissLog(options =>
                {
                    options.Formatter = args =>
                    {
                        if (args.Exception == null)
                            return args.DefaultValue;
 
                        string exceptionStr = new ExceptionFormatter().Format(args.Exception, args.Logger);

                        return string.Join(Environment.NewLine, args.DefaultValue, exceptionStr);
                    };
                });
        });

        services.AddHttpContextAccessor();
    }

    public static void UseLoggingConfiguration(this IApplicationBuilder app, WebApplicationBuilder builder)
    {
        app.UseKissLogMiddleware(options => {
            options.Listeners.Add(new RequestLogsApiListener(new Application(
                    builder.Configuration["KissLog.OrganizationId"],
                    builder.Configuration["KissLog.ApplicationId"])
            )
            {
                ApiUrl = builder.Configuration["KissLog.ApiUrl"],
                Interceptor = new HealthCheckInterceptorConfig()
            });
        });
    }
}