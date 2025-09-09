using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rebus.Config;
using Rebus.ServiceProvider;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi
{
    public static class ServiceCollectionExtensions
    {
        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.AddDefaultLogging();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                });
            builder.Services.AddEndpointsApiExplorer();

            builder.AddBasicHealthChecks();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DefaultContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                )
            );

            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.RegisterDependencies();

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(Program).Assembly);
            }, typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            builder.Services.AddRebus(configure => configure
                .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost:5673", "queue-sale")),
                onCreated: async bus =>
                {
                    await bus.Subscribe<SaleCreatedEvent>();
                    await bus.Subscribe<SaleUpdatedEvent>();
                    await bus.Subscribe<SaleCancelledEvent>();
                    await bus.Subscribe<SaleItemCancelledEvent>();
                }
            );
            builder.Services.AddRebusHandler<SaleCreatedEventHandler>();
            builder.Services.AddRebusHandler<SaleUpdatedEventHandler>();
            builder.Services.AddRebusHandler<SaleCancelledEventHandler>();
            builder.Services.AddRebusHandler<SaleItemCancelledEventHandler>();

            return builder;
        }
    }
}
