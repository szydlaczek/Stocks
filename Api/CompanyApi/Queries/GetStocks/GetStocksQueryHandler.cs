using CompanyApi.Context;
using CompanyApi.Dtos;
using CompanyApi.Errors;
using CompanyApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CompanyApi.Queries.GetStocks
{
    public class GetStocksQueryHandler : IRequestHandler<GetStocksQuery, ReadOnlyCollection<StocksDateGroup>>
    {
        private readonly ApplicationDbContext _context;

        public GetStocksQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReadOnlyCollection<StocksDateGroup>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies.AsNoTracking()
                .Where(c => c.Isin == request.CompanyIsin)
                .FirstOrDefaultAsync();

            if (company is null)
                throw new NotFoundException($"{nameof(Company)} with isin {request.CompanyIsin} doesn't exsists");

            var groupedStocks = _context.Stock.AsNoTracking()
                .Where(d => d.CompanyId == company.CompanyId)
                .OrderBy(t => t.Time).GroupBy(g => g.Time).ToList();

            var result = groupedStocks.AsQueryable().Select(StocksDateGroup.Projection(company.Name, company.Isin)).ToList();

            return result.AsReadOnly();
        }
    }
}