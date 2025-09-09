using System;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleUpdatedEvent
    {
        public Guid SaleId { get; set; }
        public DateTime UpdatedAt { get; set; }
        // outros campos relevantes
    }
}
