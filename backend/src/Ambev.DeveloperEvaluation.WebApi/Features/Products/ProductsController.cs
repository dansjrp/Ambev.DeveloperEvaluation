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

using AutoMapper;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
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
        try
        {
            var (products, totalCount) = await _mediator.Send(new GetProductsByCategoryPaginatedCommand { Category = category, Page = page, PageSize = pageSize, OrderBy = order });
            if (products == null || !products.Any())
                return ResourceNotFound("Products not found", $"No products found for category '{category}'");
            var response = new PaginatedResponse<Product>
            {
                Data = products,
                CurrentPage = page,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex.Message);
        }
    }

    // POST: api/products
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
    {
        try
        {
            var command = new CreateProductCommand
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                Category = request.Category,
                Image = request.Image,
                Rating = request.Rating
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
            return Created(string.Empty, new ApiResponseWithData<CreateProductResponse>
            {
                Success = true,
                Message = "Product created successfully",
                Data = response
            });
        }
        catch (Exception ex)
        {
            return InternalServerError(ex.Message);
        }
    }

    // PUT: api/products/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductRequest request)
    {
        try
        {
            var command = new UpdateProductCommand
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                Category = request.Category,
                Image = request.Image,
                Rating = request.Rating
            };
            var result = await _mediator.Send(command);
            if (result == null)
                return ResourceNotFound("Product not found", $"The product with ID {id} does not exist in our database");
            var response = new UpdateProductResponse
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                Price = result.Price,
                Category = result.Category,
                Image = result.Image
            };
            return Ok(new ApiResponseWithData<UpdateProductResponse>
            {
                Success = true,
                Message = "Product updated successfully",
                Data = response
            });
        }
        catch (Exception ex)
        {
            return InternalServerError(ex.Message);
        }
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var command = new DeleteProductCommand { Id = id };
        try
        {
            var result = await _mediator.Send(command);
            if (!result)
                return ResourceNotFound("Product not found", $"The product with ID {id} does not exist in our database");
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Produto deletado com sucesso."
            });
        }
        catch (Exception ex)
        {
            return InternalServerError(ex.Message);
        }
    }
}
