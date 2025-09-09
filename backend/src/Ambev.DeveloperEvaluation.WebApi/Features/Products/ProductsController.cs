using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using MediatR;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/products/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var result = await _mediator.Send(new GetProductCommand { Id = id });
        return Ok(result);
    }

    // GET: api/products?page=1&pageSize=10&order=title asc,price desc
    [HttpGet]
    public async Task<IActionResult> GetAllProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? order = null)
    {
        var (products, totalCount) = await _mediator.Send(new GetProductsPaginatedCommand { Page = page, PageSize = pageSize, OrderBy = order });
        var response = new PaginatedResponse<Product>
        {
            Data = products,
            CurrentPage = page,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        };
        return Ok(response);
    }

    // GET: api/products/categories
    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _mediator.Send(new GetAllCategoriesCommand());
        return Ok(result);
    }

    // GET: api/products/category/{category}?page=1&pageSize=10&order=title asc,price desc
    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetProductsByCategory(string category, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? order = null)
    {
        var (products, totalCount) = await _mediator.Send(new GetProductsByCategoryPaginatedCommand { Category = category, Page = page, PageSize = pageSize, OrderBy = order });
        var response = new PaginatedResponse<Product>
        {
            Data = products,
            CurrentPage = page,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        };
        return Ok(response);
    }

    // POST: api/products
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
    {
        var command = new CreateProductCommand
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            Category = request.Category,
            // Adicione outros campos conforme necessário
        };
        var result = await _mediator.Send(command);
        var response = new CreateProductResponse
        {
            Id = result.Id,
            Title = result.Title,
            Description = result.Description,
            Price = result.Price,
            Category = result.Category,
            Image = result.Image
        };
        return CreatedAtAction(nameof(GetProductById), new { id = response.Id }, response);
    }

    // PUT: api/products/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductRequest request)
    {
        var command = new UpdateProductCommand
        {
            Id = id,
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            Category = request.Category,
            // Adicione outros campos conforme necessário
        };
        var result = await _mediator.Send(command);
        var response = new UpdateProductResponse
        {
            Id = result.Id,
            Title = result.Title,
            Description = result.Description,
            Price = result.Price,
            Category = result.Category,
            Image = result.Image
        };
        return Ok(response);
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var command = new DeleteProductCommand { Id = id };
        var result = await _mediator.Send(command);
        var response = new DeleteProductResponse
        {
            Success = result,
            Message = result ? "Produto deletado com sucesso." : "Produto não encontrado."
        };
        if (result)
            return Ok(response);
        else
            return NotFound(response);
    }
}
