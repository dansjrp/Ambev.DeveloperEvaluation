using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetAllCategoriesCommand : IRequest<List<string>>
{
}
