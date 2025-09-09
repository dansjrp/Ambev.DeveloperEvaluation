using System;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleItemCancelledEvent
    {
        public Guid SaleId { get; set; }
        public Guid ItemId { get; set; }
        public DateTime CancelledAt { get; set; }
        // outros campos relevantes
    }
}
