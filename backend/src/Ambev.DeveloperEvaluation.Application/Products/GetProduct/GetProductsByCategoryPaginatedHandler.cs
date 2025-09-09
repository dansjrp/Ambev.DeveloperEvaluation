using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Common;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductsByCategoryPaginatedHandler : IRequestHandler<GetProductsByCategoryPaginatedCommand, (IEnumerable<Product> Products, int TotalCount)>
{
    private readonly IProductRepository _productRepository;

    public GetProductsByCategoryPaginatedHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount)> Handle(GetProductsByCategoryPaginatedCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Category))
            return (Enumerable.Empty<Product>(), 0);

        var (products, totalCount) = await _productRepository.GetPaginatedByCategoryAsync(request.Category, request.Page, request.PageSize, request.OrderBy, cancellationToken);
        return (products, totalCount);
    }
}
