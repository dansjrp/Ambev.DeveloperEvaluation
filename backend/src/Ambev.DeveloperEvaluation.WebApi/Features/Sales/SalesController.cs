using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using MediatR;
using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : BaseController
    {
        private readonly ISaleRepository _saleRepository;
        private readonly DbContext _context;
        private readonly IMediator _mediator;

        public SalesController(ISaleRepository saleRepository, DbContext context, IMediator mediator)
        {
            _saleRepository = saleRepository;
            _context = context;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetSalesPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? order = null)
        {
            var (sales, totalCount) = await _mediator.Send(new Application.Sales.GetSale.GetSalesPaginatedCommand { Page = page, PageSize = pageSize, OrderBy = order });
            var response = new {
                Data = sales,
                CurrentPage = page,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
            return Ok(response);
        }

        [HttpGet("{number}")]
        public async Task<IActionResult> GetByNumber(int number)
        {
            try
            {
                var result = await _mediator.Send(new Application.Sales.GetSale.GetSaleCommand { Number = number });
                if (result == null)
                    return ResourceNotFound("Sale not found", $"The sale with Number {number} does not exist in our database");
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return ResourceNotFound("Sale not found", ex.Message);
            }
        }

        [HttpPut("{saleId}/item/{itemId}")]
        public async Task<IActionResult> UpdateSaleItem(Guid saleId, Guid itemId, [FromBody] UpdateSaleItemRequest request)
        {
            try
            {
                var command = new Application.Sales.UpdateSale.UpdateSaleCommand
                {
                    SaleId = saleId,
                    ProductId = itemId,
                    Quantity = request.Quantity,
                    Cancelled = request.Cancelled
                };
                var result = await _mediator.Send(command);
                if (!result)
                    return ResourceNotFound("SaleItem not found or Sale not found", $"SaleItem with id {itemId} not found for sale {saleId}.");
                return Ok(new { Message = "SaleItem updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return ValidationError(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return InternalServerError(ex.Message);
            }
        }
    }
}
