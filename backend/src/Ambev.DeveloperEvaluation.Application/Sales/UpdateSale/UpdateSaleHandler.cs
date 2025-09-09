using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, bool>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleService _saleService;

        public UpdateSaleHandler(ISaleRepository saleRepository, ISaleService saleService)
        {
            _saleRepository = saleRepository;
            _saleService = saleService;
        }

        public async Task<bool> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(request.SaleId, "SaleItems");
            if (sale == null)
                return false;

            if (sale.SaleItems == null || sale.SaleItems.Count == 0)
                return false;

            var saleItem = sale.SaleItems?.FirstOrDefault(si => si.ProductId == request.ProductId);
            if (saleItem == null)
                return false;

            saleItem.Quantity = request.Quantity;
            saleItem.Cancelled = request.Cancelled;
            sale.Total = 0m;

            foreach (var item in sale.SaleItems)
            {
                if (item.Cancelled) continue;

                var desconto = _saleService.CalculateDiscount(item.Quantity, item.Price);
                var totalItem = (item.Quantity * item.Price) - desconto;

                sale.Total += totalItem;
            }

            await _saleRepository.UpdateAsync(sale);
            
            return true;
        }
    }
}
