using IrBusWebService.Models;
using System.Collections.Generic;

namespace IrBusWebService.Results
{
    public class GetCitiesResult : ErrorStatusResult
    {
        public IEnumerable<BusCity> BusCities { get; set; }
    }
}
