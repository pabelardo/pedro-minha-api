using Api.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Api.Configuration;

public static class ApiConfig
{
    public static void AddApiConfig(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true; //Quando não tiver uma versão 1, 2 ou 3... ele irá assumir a versão default
            options.DefaultApiVersion = new ApiVersion(1, 0); //Essa é a versão default 
            options.ReportApiVersions = true; //Serve para alertar que existe uma versão mais atualizada na API
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV"; //'v' = versão; Os outros V's significam respectivamente ao major version, minor version e o patch
            options.SubstituteApiVersionInUrl = true; //Se não for especificado qual versão você quer iniciar a API, ele joga na primeira versão
        });

        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

        // services.AddCors(options =>
        // {
        //     options.AddPolicy("Development",
        //         builder =>
        //             builder
        //             .AllowAnyOrigin()
        //             .AllowAnyMethod()
        //             .AllowAnyHeader());

        //     options.AddPolicy("Production",
        //         builder =>
        //             builder
        //                 .WithMethods("GET")
        //                 .WithOrigins("http://desenvolvedor.io")
        //                 .SetIsOriginAllowedToAllowWildcardSubdomains()
        //                 //.WithHeaders(HeaderNames.ContentType, "x-custom-header")
        //                 .AllowAnyHeader());
        // });
    }

    public static void UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            //app.UseCors("Development");
            app.UseDeveloperExceptionPage();
        }
        else
        {
            //app.UseCors("Production"); // Usar apenas nas demos => Configuração Ideal: Production
            app.UseHsts();
        }

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseStaticFiles();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.MapHealthChecks("/api/hc-i", new HealthCheckOptions
            {
                // WriteResponse is a delegate used to write the response.
                ResponseWriter = (httpContext, result) => {
                    httpContext.Response.ContentType = "application/json";

                    var json = new JObject(
                        new JProperty("status", result.Status.ToString()),
                        new JProperty("results", new JObject(result.Entries.Select(pair =>
                            new JProperty(pair.Key, new JObject(
                                new JProperty("status", pair.Value.Status.ToString()),
                                new JProperty("description", pair.Value.Description),
                                new JProperty("data", new JObject(pair.Value.Data.Select(
                                    p => new JProperty(p.Key, p.Value))))))))));
                    return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
                }
            });

            endpoints.MapHealthChecks("/api/hc", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            endpoints.MapHealthChecksUI(options =>
            {
                options.UIPath = "/api/hc-ui";
                options.ResourcesPath = "/api/hc-ui-resources";
                options.AddCustomStylesheet("hcStyle.css");
                options.UseRelativeApiPath = false;
                options.UseRelativeResourcesPath = false;
                options.UseRelativeWebhookPath = false;
            });

        });
    }
}