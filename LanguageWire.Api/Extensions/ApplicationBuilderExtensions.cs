using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;

namespace LanguageWire.Api.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
      public static IApplicationBuilder UseSwaggerMiddlewares(this IApplicationBuilder app, string title)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(options =>
            {
                options.SerializeAsV2 = false;
                options.RouteTemplate = "swagger/{documentName}/swagger.json";
                options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    if (TryGetBasePath(httpReq, out var basePath))
                    {
                        var isLocalhost = httpReq.Host.Value.IndexOf("localhost", StringComparison.OrdinalIgnoreCase) != -1;
                        var scheme = isLocalhost ? httpReq.Scheme : "https";
                        swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{scheme}://{httpReq.Host.Value}{basePath}" } };
                    }
                });
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
//            var authConfig = app.ApplicationServices.GetRequiredService<AuthConfig>();
            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = title;
                options.SwaggerEndpoint("./swagger/v1/swagger.json", title);
                options.RoutePrefix = string.Empty;

                //options.OAuthClientId(authConfig.ClientId);
                options.OAuthRealm(string.Empty);
                options.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
                {
                    { "nonce", $"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}" },
                    { "state", $"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}" },
                });
            });

            return app;
        }

        private static bool TryGetBasePath(HttpRequest httpReq, out string baseUri)
        {
            var keys = new[] { "X-Original-URI", "X-Original-URL", "Referer" };
            if (TryGetHeader(httpReq.Headers, keys, out var originalUri))
            {
                try
                {
                    baseUri = new Uri(new Uri(originalUri), ".").AbsolutePath;
                    var swaggerRouteIndex = baseUri.IndexOf("/swagger/", StringComparison.OrdinalIgnoreCase);
                    if (swaggerRouteIndex > -1)
                    {
                        baseUri = baseUri.Substring(0, swaggerRouteIndex + 1);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to get baseUri. OriginalUri:{originalUri}", ex);
                }
            }

            baseUri = null;
            return false;
        }

        private static bool TryGetHeader(IHeaderDictionary headers, string[] keys, out string value)
        {
            foreach (var key in keys)
            {
                if (headers.TryGetValue(key, out var headerValue) && !string.IsNullOrEmpty(headerValue))
                {
                    value = headerValue.ToString();
                    return true;
                }
            }

            value = null;
            return false;
        }  
    }
}