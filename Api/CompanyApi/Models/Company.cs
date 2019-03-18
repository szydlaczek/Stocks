using System.Collections.Generic;

namespace CompanyApi.Models
{
    public class Company
    {
        public int CompanyId { get; protected set; }
        public string Name { get; protected set; }
        public string Isin { get; protected set; }
        public virtual IEnumerable<Stock> Stocks => _stocks.AsReadOnly();

        private readonly List<Stock> _stocks =
        new List<Stock>();

        public Company(string name, string isin)
        {
            Name = name;
            Isin = isin;
        }

        public void AddStock(Stock stock)
        {
            _stocks.Add(stock);
        }
    }
}