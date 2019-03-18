using CompanyApi.Dtos;

using MediatR;
using System.Collections.ObjectModel;

namespace CompanyApi.Queries.GetStocks
{
    public class GetStocksQuery : IRequest<ReadOnlyCollection<StocksDateGroup>>
    {
        public string CompanyIsin { get; }

        public GetStocksQuery(string companyIsin)
        {
            CompanyIsin = companyIsin;
        }
    }
}