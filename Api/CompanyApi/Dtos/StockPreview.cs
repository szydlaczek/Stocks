namespace CompanyApi.Dtos
{
    public class StockPreview
    {
        public string Isin { get; set; }
        public string CompanyName { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double Hight { get; set; }
        public double Low { get; set; }
        //public static Expression<Func<IGrouping<DateTime,Stock>, StockPreview>> Projection(string companyName, string isin)
        //{
        //    return p => new StockPreview
        //    {
        //        CompanyName = companyName,
        //        Isin=isin,
        //        Open = p.First().Value,
        //        Hight = p.Max(d => d.Value),
        //        Low = p.Min(d => d.Value),
        //        Close = p.OrderByDescending(d => d.Time).FirstOrDefault().Value
        //    };
        //}
    }
}