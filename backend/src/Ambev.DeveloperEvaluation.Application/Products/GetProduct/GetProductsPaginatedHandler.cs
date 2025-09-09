using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Common;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductsPaginatedHandler : IRequestHandler<GetProductsPaginatedCommand, (IEnumerable<Product> Products, int TotalCount)>
{
    private readonly IProductRepository _productRepository;

    public GetProductsPaginatedHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount)> Handle(GetProductsPaginatedCommand request, CancellationToken cancellationToken)
    {
    var (products, totalCount) = await _productRepository.GetPaginatedAsync(request.Page, request.PageSize, request.OrderBy, cancellationToken);
    return (products, totalCount);
    }
}
