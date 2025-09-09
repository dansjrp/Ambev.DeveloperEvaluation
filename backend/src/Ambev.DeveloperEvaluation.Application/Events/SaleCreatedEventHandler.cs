using System.Threading.Tasks;
using Rebus.Handlers;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Events
{
    public class SaleCreatedEventHandler : IHandleMessages<SaleCreatedEvent>
    {
        private readonly Serilog.ILogger _logger;

        public SaleCreatedEventHandler()
        {
            _logger = Serilog.Log.ForContext<SaleCreatedEventHandler>();
        }

        public async Task Handle(SaleCreatedEvent message)
        {
            _logger.Information("SaleCreatedEvent recebido: SaleId={SaleId}, CreatedAt={CreatedAt}", message.SaleId, message.CreatedAt);
            await Task.CompletedTask;
        }
    }
}
