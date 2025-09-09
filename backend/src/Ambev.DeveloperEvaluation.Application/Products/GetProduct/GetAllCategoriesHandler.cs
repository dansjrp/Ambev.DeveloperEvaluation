using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesCommand, List<string>>
{
    private readonly IProductRepository _productRepository;

    public GetAllCategoriesHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<string>> Handle(GetAllCategoriesCommand request, CancellationToken cancellationToken)
    {
        return await _productRepository.GetAllCategoriesAsync(cancellationToken);
    }
}
