using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using System.Linq;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSalesPaginatedHandler : IRequestHandler<GetSalesPaginatedCommand, (List<SaleDto> Sales, int TotalCount)>
    {
        private readonly ISaleRepository _saleRepository;

        public GetSalesPaginatedHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<(List<SaleDto> Sales, int TotalCount)> Handle(GetSalesPaginatedCommand request, CancellationToken cancellationToken)
        {
            var (sales, totalCount) = await _saleRepository.GetPaginatedAsync(request.Page, request.PageSize, request.OrderBy);
            var salesDto = sales.Select(s => SaleDto.FromEntity(s)).ToList();
            return (salesDto, totalCount);
        }
    }
}
