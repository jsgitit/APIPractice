﻿using Asp.Versioning.ApiExplorer;
using CompanyWebApi.Configurations;
using CompanyWebApi.Services.Swagger.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CompanyWebApi.Services.Swagger
{
    /// <summary>
    /// Configures the Swagger generation options
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiProvider;
        private readonly SwaggerConfig _swaggerConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerGenOptions"/> class
        /// </summary>
        /// <param name="apiProvider">The <see cref="IApiVersionDescriptionProvider">apiProvider</see> used to generate Swagger documents.</param>
        /// <param name="swaggerConfig"></param>
        public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider apiProvider, IOptions<SwaggerConfig> swaggerConfig)
        {
            _apiProvider = apiProvider ?? throw new ArgumentNullException(nameof(apiProvider));
            _swaggerConfig = swaggerConfig.Value;
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // Add a custom operation filter which sets default values
            options.OperationFilter<SwaggerDefaultValues>();

            // Add a swagger document for each discovered API version
            // Note: you might choose to skip or document deprecated API versions differently
            foreach (var description in _apiProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }

            // Add JWT Bearer Authorization
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            // Add Security Requirement
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            // Include Document file for CompanyWebApi project
            options.IncludeXmlComments(GetXmlCommentsPath(), true);

            // Include Document file for CompanyWebApi.Contracts project
            options.IncludeXmlComments(GetXmlCommentsPathForCompanyWebApiContracts());

            // Provide a custom strategy for generating the unique Id's
            options.CustomSchemaIds(x => x.FullName);

            // Resolve conflicts between actions with the same route
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        }

        private static string GetXmlCommentsPath()
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            return xmlPath;
        }

        private static string GetXmlCommentsPathForCompanyWebApiContracts()
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.Contracts.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            return xmlPath;
        }

        /// <summary>
        /// Create API version
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = _swaggerConfig.Title,
                Version = description.ApiVersion.ToString(),
                Description = _swaggerConfig.Description,
                Contact = new OpenApiContact
                {
                    Name = _swaggerConfig.ContactName,
                    Email = _swaggerConfig.ContactEmail,
                    Url = new Uri(_swaggerConfig.ContactUrl)
                },
                License = new OpenApiLicense
                {
                    Name = _swaggerConfig.LicenseName,
                    Url = new Uri(_swaggerConfig.LicenseUrl)
                },
                // Add a logo to ReDoc page
                Extensions = new Dictionary<string, IOpenApiExtension>
                {
                    {
                        "x-logo", new OpenApiObject
                        {
                            {"url", new OpenApiString("/wwwroot/swagger/company-logo-redoc.png")}
                        }
                    }
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += " ** THIS API VERSION HAS BEEN DEPRECATED!";
            }

            return info;
        }
    }
}