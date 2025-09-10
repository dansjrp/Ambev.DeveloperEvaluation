using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using MediatR;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CloseCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

using Ambev.DeveloperEvaluation.WebApi.Common;

[ApiController]
[Route("api/[controller]")]
public class CartsController : BaseController
{
    private readonly IMediator _mediator;

    public CartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // DELETE: api/carts/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCart(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new DeleteCartCommand { Id = id });
            if (!result)
                return ResourceNotFound("Cart not found", $"The cart with ID {id} does not exist in our database");
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Cart deleted successfully."
            });
        }
        catch (Exception ex)
        {
            return InternalServerError(ex.Message);
        }
    }
    // POST: api/carts
    [HttpPost]
    public async Task<IActionResult> CreateCart([FromBody] CreateCartRequest request)
    {
        try
        {
            var command = new CreateCartCommand
            {
                UserId = request.UserId,
                Items = request.Items.ConvertAll(i => new Application.Carts.CreateCart.CartItemDto { ProductId = i.ProductId, Quantity = i.Quantity })
            };
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCartById), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException ex)
        {
            return ResourceNotFound("Resource not found", ex.Message);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex.Message);
        }
    }

    // PUT: api/carts/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCart(Guid id, [FromBody] UpdateCartRequest request)
    {
        try
        {
            var command = new UpdateCartCommand
            {
                Id = id,
                Items = request.Items.ConvertAll(i => new Application.Carts.UpdateCart.CartItemDto { ProductId = i.ProductId, Quantity = i.Quantity })
            };
            var result = await _mediator.Send(command);
            if (result == null)
                return ResourceNotFound("Cart not found", $"The cart with ID {id} does not exist in our database");
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "One or more products were not found.",
                Details = ex.Message
            });
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Validation error.",
                Details = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage))
            });
        }
        catch (Exception ex)
        {
            return InternalServerError(ex.Message);
        }
    }

    // GET: api/carts/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCartById(Guid id)
    {
        var result = await _mediator.Send(new GetCartByIdCommand { Id = id });
        if (result == null)
            return ResourceNotFound("Cart not found", $"The cart with ID {id} does not exist in our database");
        return Ok(result);
    }

    // GET: api/carts?page=1&pageSize=10&order=userId asc
    [HttpGet]
    public async Task<IActionResult> GetCartsPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? order = null)
    {
        var (carts, totalCount) = await _mediator.Send(new GetCartsPaginatedCommand { Page = page, PageSize = pageSize, OrderBy = order });
        var response = new GetCartsPaginatedResponse
        {
            Data = new List<GetCartByIdResponse>(),
            CurrentPage = page,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        };
        foreach (var c in carts)
        {
            response.Data.Add(new GetCartByIdResponse
            {
                Id = c.Id,
                UserId = c.UserId,
                UserName = c.User?.Username ?? "",
                Items = c.Products.ConvertAll(i => new CartItemResponse { ProductId = i.ProductId, ProductName = i.Product?.Title ?? "", Quantity = i.Quantity })
            });
        }
        return Ok(response);
    }

    // POST: api/carts/{cartId}/close
    [HttpPost("{cartId}/close")]
    public async Task<IActionResult> CloseCart(Guid cartId, [FromBody] CloseCartRequest request, [FromServices] IMediator mediator)
    {
        var command = new Application.Sales.CreateSale.CreateSaleCommand
        {
            CartId = cartId,
            Branch = request.Branch
        };

        var sale = await mediator.Send(command);
        if (sale == null)
            return ResourceNotFound("Sale not created", $"Could not create sale for cart {cartId}.");

        var response = new CloseCart.CloseCartResponse
        {
            SaleId = sale.Id,
            Number = sale.Number,
            Date = sale.Date,
            UserId = sale.UserId,
            Branch = sale.Branch,
            Total = sale.Total,
            Items = sale.SaleItems?.Select(i => new CloseCart.CloseCartSaleItemResponse
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price,
                Discounts = i.Discounts,
                TotalPrice = i.TotalPrice,
                Cancelled = i.Cancelled
            }).ToList() ?? new List<CloseCart.CloseCartSaleItemResponse>()
        };
        return Ok(response);
    }
}
