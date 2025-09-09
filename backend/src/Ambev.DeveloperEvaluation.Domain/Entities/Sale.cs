using System;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public decimal Total { get; set; }
        public string Branch { get; set; } = string.Empty;
    }
}
