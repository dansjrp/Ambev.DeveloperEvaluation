using System.Threading.Tasks;
using Rebus.Handlers;
using Ambev.DeveloperEvaluation.Domain.Events;
using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Events
{
    public class SaleItemCancelledEventHandler : IHandleMessages<SaleItemCancelledEvent>
    {
        private readonly ILogger _logger;

        public SaleItemCancelledEventHandler()
        {
            _logger = Log.ForContext<SaleItemCancelledEventHandler>();
        }

        public async Task Handle(SaleItemCancelledEvent message)
        {
            _logger.Information("SaleItemCancelledEvent recebido: SaleId={SaleId}, ItemId={ItemId}, CancelledAt={CancelledAt}", message.SaleId, message.ItemId, message.CancelledAt);
            await Task.CompletedTask;
        }
    }
}
