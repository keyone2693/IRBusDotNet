using IrBusWebService.Models;
using System.Collections.Generic;

namespace IrBusWebService.Results
{
    public class GetServicesResult : ErrorStatusResult
    {
        public IEnumerable<BusServices> BusServices { get; set; }
    }
}
