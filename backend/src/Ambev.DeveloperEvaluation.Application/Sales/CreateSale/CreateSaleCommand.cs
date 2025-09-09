using MediatR;
using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommand : IRequest<Sale>
    {
        public Guid CartId { get; set; }
        public string Branch { get; set; } = string.Empty;
    }
}
