using System.Threading.Tasks;
using Rebus.Handlers;
using Ambev.DeveloperEvaluation.Domain.Events;
using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Events
{
    public class SaleCancelledEventHandler : IHandleMessages<SaleCancelledEvent>
    {
        private readonly ILogger _logger;
        private readonly EventAuditMongoRepository _auditRepo;

        public SaleCancelledEventHandler(EventAuditMongoRepository auditRepo)
        {
            _logger = Log.ForContext<SaleCancelledEventHandler>();
            _auditRepo = auditRepo;
        }

        public async Task Handle(SaleCancelledEvent message)
        {
            _logger.Information("SaleCancelledEvent recebido: SaleId={SaleId}, CancelledAt={CancelledAt}", message.SaleId, message.CancelledAt);
            await _auditRepo.StoreEventAsync(message);
            await Task.CompletedTask;
        }
    }
}
