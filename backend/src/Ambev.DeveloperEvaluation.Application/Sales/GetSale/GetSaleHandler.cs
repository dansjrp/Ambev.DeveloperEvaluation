using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleByIdResult>
    {
        private readonly ISaleRepository _saleRepository;
        public GetSaleHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<GetSaleByIdResult> Handle(GetSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByNumberAsync(request.Number, "SaleItems");

            if (sale == null)
                throw new KeyNotFoundException($"Sale with Number {request.Number} not found");

            var response = new GetSaleByIdResult
            {
                Id = sale.Id,
                Number = sale.Number,
                Date = sale.Date,
                UserId = sale.UserId,
                Branch = sale.Branch,
                Total = sale.Total,
                Items = sale.SaleItems?.Select(i => new GetSaleItemResult
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Discounts = i.Discounts,
                    TotalPrice = i.TotalPrice,
                    Cancelled = i.Cancelled
                }).ToList() ?? new List<GetSaleItemResult>()
            };
            return response;
        }
    }
}
