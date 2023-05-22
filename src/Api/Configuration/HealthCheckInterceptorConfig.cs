using KissLog;
using HttpRequest = KissLog.Http.HttpRequest;

namespace Api.Configuration
{
    public class HealthCheckInterceptorConfig : ILogListenerInterceptor
    {
        public bool ShouldLog(FlushLogArgs args, ILogListener listener)
        {
            if (args.HttpProperties.Request.Url.LocalPath != "/api/healthcheck") return true;

            return args.HttpProperties.Response.StatusCode != 200;
        }

        public bool ShouldLog(HttpRequest httpRequest, ILogListener listener)
        {
            return true;
        }

        public bool ShouldLog(LogMessage message, ILogListener listener)
        {
            return true;
        }
    }
}
