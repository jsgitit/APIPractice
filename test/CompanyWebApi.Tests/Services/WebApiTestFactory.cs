using CompanyWebApi.Persistence.DbContexts;
using CompanyWebApi.Persistence.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebMotions.Fake.Authentication.JwtBearer;

namespace CompanyWebApi.Tests.Services
{
    /// <summary>
    /// Customized WebApplicationFactory
    /// </summary>
    public class WebApiTestFactory : WebApplicationFactory<Startup>
    {
        private readonly string _databaseFileName = Guid.NewGuid().ToString();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseContentRoot(".") // Content root directory for web host
                .UseTestServer() // Add TestServer
                .UseEnvironment("Test") // Specify the environment
                .ConfigureTestServices(services =>
                {
                    // Remove the ApplicationDbContext registration
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add ApplicationDbContext using a unique SQLite in-memory database for testing, NOT the EntityFrameworkInMemoryDatabase
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseSqlite($"DataSource=file:{_databaseFileName}?mode=memory&cache=shared", sqliteOptions =>
                        {
                            sqliteOptions.UseRelationalNulls();
                            sqliteOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                    });

                    var serviceProvider = services.BuildServiceProvider();

                    // Create a scope to obtain a reference to the database context (ApplicationDbContext)
                    using (var serviceScope = serviceProvider.CreateScope())
                    {
                        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<WebApiTestFactory>>();
                        try
                        {
                            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                            dbContext.Database.OpenConnection();

                            // Use Migrate() instead of EnsureCreated() to evolve the database schema
                            // Migrate() is preferred when evolving the schema and bringing both schema and data to the current state
                            // While dbContext may contain initial seed data, migrations are expected to transition data from seed data to the evolved schema
                            dbContext.Database.Migrate();

                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"An error occurred seeding the database with test messages. Error: {ex.Message}");
                        }
                    }

                    // Add fake Jwt authentication
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                    }).AddFakeJwtBearer();
                });

            // Call base Configuration
            base.ConfigureWebHost(builder);
        }
    }
}