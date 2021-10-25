using System;
using System.IO;
using LanguageWire.Api.Business;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LanguageWire.Api.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger<TStartup>(this IServiceCollection services, string title)
        {
            services.AddSingleton<ITranslatorBusiness, TranslatorBusiness>();
            services
                .AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = title,
                        Version = "v1"
                    });

                    var xmlFile = $"{typeof(TStartup).Assembly.GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                    options.CustomOperationIds(apiDesc =>
                    {
                        return apiDesc.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name : null;
                    });
                });

            return services;
        }
    }
}