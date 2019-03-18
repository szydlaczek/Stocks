using CompanyApi.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CompanyApi.Dtos
{
    public class StocksDateGroup
    {
        public DateTime Date { get; set; }
        public StockPreview Stock { get; set; }

        public static Expression<Func<IGrouping<DateTime, Stock>, StocksDateGroup>> Projection(string companyName, string isin)
        {
            return p => new StocksDateGroup
            {
                Date = p.Key,

                Stock = new StockPreview
                {
                    CompanyName = companyName,
                    Isin = isin,
                    Open = p.First().Value,
                    Hight = p.Max(d => d.Value),
                    Low = p.Min(d => d.Value),
                    Close = p.OrderByDescending(d => d.Time).FirstOrDefault().Value
                }
            };
        }
    }
}