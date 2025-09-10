using System.Threading.Tasks;
using Rebus.Handlers;
using Ambev.DeveloperEvaluation.Domain.Events;
using Serilog;

namespace Ambev.DeveloperEvaluation.Application.Events
{
    public class SaleUpdatedEventHandler : IHandleMessages<SaleUpdatedEvent>
    {
        private readonly Serilog.ILogger _logger;
        private readonly EventAuditMongoRepository _auditRepo;

        public SaleUpdatedEventHandler(EventAuditMongoRepository auditRepo)
        {
            _logger = Serilog.Log.ForContext<SaleUpdatedEventHandler>();
            _auditRepo = auditRepo;
        }

        public async Task Handle(SaleUpdatedEvent message)
        {
            _logger.Information("SaleUpdatedEvent recebido: SaleId={SaleId}, UpdatedAt={UpdatedAt}", message.SaleId, message.UpdatedAt);
            await _auditRepo.StoreEventAsync(message);
            await Task.CompletedTask;
        }
    }
}
