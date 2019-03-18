using CompanyApi.Filters;
using CompanyApi.Queries.GetStocks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CompanyApi.Controllers
{
    [Route("api/[controller]")]
    public class CompanyController : Controller
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{isin}/{stocks}/{all}")]
        [Throttle(Name = "ThrottleTest", SecondsToBlock = 600, RequestLimit = 5, RequestLimitTime = 300)]
        public async Task<IActionResult> GetStocksByIsin(string isin)
        {
            var result = await _mediator.Send(new GetStocksQuery(isin));
            var json = JsonConvert.SerializeObject(result);
            return Ok(json);
        }
    }
}