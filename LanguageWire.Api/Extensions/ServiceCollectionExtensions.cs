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

                    // Use method name as operationId
                    options.CustomOperationIds(apiDesc =>
                    {
                        return apiDesc.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name : null;
                    });

                    // TODO: change configuration because this is obsolete
                    // Use the "UseOneOfForPolymorphism" and "UseAllOfForInheritance" settings instead
#pragma warning disable CS0618 // Type or member is obsolete
                    options.GeneratePolymorphicSchemas();
#pragma warning restore CS0618 // Type or member is obsolete

                    //options.OperationFilter<SecurityRequirementsOperationFilter>();
                })
                .AddSwaggerGenNewtonsoftSupport();

            return services;
        }
    }
}