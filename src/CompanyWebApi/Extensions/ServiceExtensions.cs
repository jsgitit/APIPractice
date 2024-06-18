using Asp.Versioning;
using CompanyWebApi.Persistence.Repositories.Factory;
using CompanyWebApi.Services.Swagger;
using CompanyWebApi.Services.Swagger.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CompanyWebApi.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Adds service API versioning
        /// </summary>
        /// <param name="services"></param>
        public static void AddAndConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                // Specify the default API AssemblyVersion
                options.DefaultApiVersion = new ApiVersion(2, 1);
                // Use default version when version is not specified
                options.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'V'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddEndpointsApiExplorer();
        }

        /// <summary>
        /// Adds cross-origin resource sharing services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="policyName"></param>
        public static void AddCorsPolicy(this IServiceCollection services, string policyName)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(policyName,
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("X-Pagination"));
            });
        }

        /// <summary>
        /// Adds Swagger support
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static void AddSwaggerMiddleware(this IServiceCollection services)
        {
            // Configure Swagger Options
            services.AddTransient<IConfigureOptions<SwaggerUIOptions>, ConfigureSwaggerUiOptions>();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();

            // Register the Swagger generator
            services.AddSwaggerGen(options =>
            {
                // Enable Swagger annotations
                options.EnableAnnotations();

                // Application Controller's API document description information
                options.DocumentFilter<SwaggerDocumentFilter>();
            });
        }

        /// <summary>
        /// Adds repository factory
        /// </summary>
        /// <param name="services"></param>
        public static void AddRepositoryFactory(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();
        }
    }
}