using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using Ambev.DeveloperEvaluation.Domain.Events;
using Rebus.Bus;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, bool>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleService _saleService;
        private readonly IBus _bus;

        public UpdateSaleHandler(ISaleRepository saleRepository, ISaleService saleService, IBus bus)
        {
            _saleRepository = saleRepository;
            _saleService = saleService;
            _bus = bus;
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
            var wasCancelled = !saleItem.Cancelled && request.Cancelled;
            saleItem.Cancelled = request.Cancelled;
            sale.Total = 0m;

            foreach (var item in sale.SaleItems ?? Enumerable.Empty<SaleItem>())
            {
                if (item.Cancelled) continue;

                var desconto = _saleService.CalculateDiscount(item.Quantity, item.Price);
                var totalItem = (item.Quantity * item.Price) - desconto;

                sale.Total += totalItem;
            }

            await _saleRepository.UpdateAsync(sale);

            await _bus.Publish(new SaleUpdatedEvent {
                SaleId = sale.Id,
                UpdatedAt = DateTime.UtcNow
            });

            if (wasCancelled)
            {
                await _bus.Publish(new SaleItemCancelledEvent {
                    SaleId = sale.Id,
                    ItemId = saleItem.Id,
                    CancelledAt = DateTime.UtcNow
                });
            }

            if ((sale.SaleItems?.All(i => i.Cancelled) ?? false))
            {
                await _bus.Publish(new SaleCancelledEvent {
                    SaleId = sale.Id,
                    CancelledAt = DateTime.UtcNow
                });
            }

            return true;
        }
    }
}
