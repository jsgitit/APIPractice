using CompanyWebApi.Configurations;
using CompanyWebApi.Core.Auth;
using CompanyWebApi.Extensions;
using CompanyWebApi.Middleware;
using CompanyWebApi.Persistence.DbContexts;
using CompanyWebApi.Persistence.Repositories;
using CompanyWebApi.RouteConstraints;
using CompanyWebApi.Services.Filters;
using CompanyWebApi.Services.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CompanyWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services required for using options
            services.AddOptions();

            // Add the whole configuration object here
            services.AddSingleton(Configuration);

            // Add health check services
            services.AddHealthChecks();

            RegisterConfigurations(services);
            RegisterServices(services);

            // Register services required by authentication services
            ConfigureAuthentication(services, Configuration);

            services.AddCorsPolicy("EnableCORS");

            // Adds service API versioning
            services.AddAndConfigureApiVersioning();

            // Adds services for controllers
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressConsumesConstraintForFormFileParameters = true;
                    options.SuppressInferBindingSourcesForParameters = true;
                    options.SuppressModelStateInvalidFilter = true; // To disable the automatic 400 behavior, set the SuppressModelStateInvalidFilter property to true
                    options.SuppressMapClientErrors = true;
                    options.ClientErrorMapping[404].Link = "https://httpstatuses.com/404";
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                });

            // Adds Swagger support
            services.AddSwaggerMiddleware();

            services.AddRepositoryFactory();

            ConfigureDatabaseServices(services);
        }

        protected virtual void ConfigureDatabaseServices(IServiceCollection services)
        {
            // Add Database Context
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
                options.UseSqlite(Configuration.GetConnectionString("SqLiteConnectionString"), opt =>
                {
                    opt.CommandTimeout(15); // secs
                    opt.UseRelationalNulls(); // added
                    opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); // added 
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration config)
        {
            // Needed for a ReDoc logo
            const string LOGO_FILE_PATH = "wwwroot/swagger";
            var fileprovider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, LOGO_FILE_PATH));
            var requestPath = new PathString($"/{LOGO_FILE_PATH}");

            app.UseDefaultFiles(new DefaultFilesOptions
            {
                FileProvider = fileprovider,
                RequestPath = requestPath,
            });

            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = fileprovider,
                RequestPath = requestPath,
                EnableDirectoryBrowsing = false
            });

            app.UseStaticFiles();

            // Register ReDoc middleware
            app.UseReDocMiddleware(config);

            // Register Swagger and SwaggerUI middleware
            app.UseSwaggerMiddleware(config);

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();

            // For elevated security, it is recommended to remove this middleware and set your server to only listen on https. 
            // A slightly less secure option would be to redirect http to 400, 505, etc.
            app.UseHttpsRedirection();

            // NOTE** Add logging middleware(s) only when not running from integration/unit tests!
            if (!UnitTestDetector.IsRunningFromUnitTest())
            {
                // Adds request/response logging middleware
                app.UseMiddleware<RequestResponseLoggingMiddleware>();

                // Adds middleware for streamlined request logging
                app.UseSerilogRequestLogging(options =>
                {
                    // Customize the message template
                    options.MessageTemplate = "{Host} {Protocol} {RequestMethod} {RequestPath} {EndpointName} {ResponseBody} responded {StatusCode} in {Elapsed} ms";
                    options.EnrichDiagnosticContext = RequestLogHelper.EnrichDiagnosticContext;
                });
            }

            // Adds global error handling middleware
            app.UseApiExceptionHandling();

            // Adds enpoint routing middleware
            app.UseRouting();

            // Adds a CORS middleware
            app.UseCors("EnableCORS");

            // These are the important ones - order matters!
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(configure =>
            {
                configure.MapControllers();
                configure.MapDefaultControllerRoute();
                configure.MapHealthChecks("health");
                // Redirect root to Swagger UI
                configure.MapGet("", context =>
                {
                    context.Response.Redirect("./swagger/index.html", permanent: false);
                    return Task.FromResult(0);
                });
            });
        }

        /// <summary>
        /// Register a configuration instances which TOptions will bind against
        /// </summary>
        /// <param name="services"></param>
        protected void RegisterConfigurations(IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.ConstraintMap.Add("AddressType", typeof(AddressTypeRouteConstraint));
            });

            services.Configure<AuthSettings>(Configuration.GetSection(nameof(AuthSettings)));
            services.Configure<SwaggerConfig>(Configuration.GetSection(nameof(SwaggerConfig)));
        }

        /// <summary>
        /// Register services required by authentication services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        protected void ConfigureAuthentication(IServiceCollection services, IConfiguration config)
        {
            var authSettings = config.GetSection(nameof(AuthSettings)).Get<AuthSettings>();
            var key = Encoding.UTF8.GetBytes(authSettings.SecretKey);
            var signingKey = new SymmetricSecurityKey(key);
            var jwtIssuerOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtIssuerOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtIssuerOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtIssuerOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true, // Audience will be validated during token validation
                ValidAudience = jwtIssuerOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.ClaimsIssuer = jwtIssuerOptions[nameof(JwtIssuerOptions.Issuer)];
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;
                opt.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
                opt.TokenValidationParameters = tokenValidationParameters;
            });
        }

        /// <summary>
        /// Register services and middlewares
        /// </summary>
        /// <param name="services"></param>
        protected virtual void RegisterServices(IServiceCollection services)
        {
            //services.AddTransient<DbInitializer>(); no longer used
            services.AddScoped<ValidModelStateAsyncActionFilter>();

            // Register middlewares
            services.AddTransient<ApiExceptionHandlingMiddleware>();
            services.AddTransient<RequestResponseLoggingMiddleware>();

            //*********************************************************************************
            // Registering multiple implementations of the same interface IRepository<TEntity>
            //*********************************************************************************
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeAddressRepository, EmployeeAddressRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Services
            services.AddTransient<IJwtTokenHandler, JwtTokenHandler>();
            services.AddTransient<IJwtFactory, JwtFactory>();
            services.AddScoped<CompanyWebApi.Services.Authentication.V3.IUserService, CompanyWebApi.Services.Authentication.V3.UserService>();
            services.AddScoped<CompanyWebApi.Services.Authentication.V4.IUserService, CompanyWebApi.Services.Authentication.V4.UserService>();

            // Add AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
