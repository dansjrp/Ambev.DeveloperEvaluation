using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, Sale>
    {
        private readonly ISaleService _saleService;
        private readonly ISaleRepository _saleRepository;
        private readonly ICartRepository _cartRepository;

        public CreateSaleHandler(ISaleService saleService, ISaleRepository saleRepository, ICartRepository cartRepository)
        {
            _saleService = saleService;
            _saleRepository = saleRepository;
            _cartRepository = cartRepository;
        }

        public async Task<Sale> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByIdAsync(request.CartId, new string[]
                {
                    "Products.Product"
                });

            if (cart == null)
                throw new KeyNotFoundException($"Cart com ID {request.CartId} não encontrado.");

            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                UserId = cart.UserId,
                Branch = request.Branch,
                Total = 0m
            };

            foreach (var cartItem in cart.Products)
            {
                var desconto = _saleService.CalculateDiscount(cartItem.Quantity, cartItem.Product.Price);
                var totalItem = (cartItem.Quantity * cartItem.Product.Price) - desconto;

                var saleItem = new SaleItem
                {
                    Id = Guid.NewGuid(),
                    SaleId = sale.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Product.Price,
                    Discounts = desconto,
                    TotalPrice = totalItem,
                    Cancelled = false,
                    Sale = sale
                };
                sale.Total += totalItem;
                sale.SaleItems.Add(saleItem);
            }

            await _saleRepository.CreateAsync(sale);
            return sale;
        }
    }
}
