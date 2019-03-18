using System;

namespace CompanyApi.Models
{
    public class Stock
    {
        public int StockId { get; protected set; }
        public double Value { get; protected set; }
        public DateTime Time { get; protected set; }
        public int CompanyId { get; protected set; }
        public Company Company { get; protected set; }

        public Stock(double value, DateTime time)
        {
            Value = value;
            Time = time;
        }

        protected Stock()
        {
        }
    }
}