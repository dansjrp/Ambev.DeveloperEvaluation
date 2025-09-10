using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.ORM;

namespace Ambev.DeveloperEvaluation.Integration
{
    public class TestContainerFixture : IAsyncLifetime
    {
        public readonly PostgreSqlContainer Postgres;
        public HttpClient Client { get; private set; } = null!;

        public TestContainerFixture()
        {
            Postgres = new PostgreSqlBuilder()
                .WithDatabase("developer_evaluation")
                .WithUsername("developer")
                .WithPassword("ev@luAt10n")
                .Build();
        }

        public async Task InitializeAsync()
        {
            await Postgres.StartAsync();

            // Executa as migrations
            var optionsBuilder = new DbContextOptionsBuilder<DefaultContext>();
            optionsBuilder.UseNpgsql(Postgres.GetConnectionString(), b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM"));
            using (var context = new DefaultContext(optionsBuilder.Options))
            {
                await context.Database.MigrateAsync();
            }

            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        var settings = new List<KeyValuePair<string, string?>>
                        {
                            new KeyValuePair<string, string?>("ConnectionStrings:DefaultConnection", Postgres.GetConnectionString())
                        };
                        config.AddInMemoryCollection(settings);
                    });
                });
            Client = factory.CreateClient();
        }

        public async Task DisposeAsync()
        {
            await Postgres.DisposeAsync();
        }
    }
}
