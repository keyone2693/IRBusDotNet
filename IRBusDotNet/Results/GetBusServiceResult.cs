using IrBusWebService.Models.BusServiceGoF;
using System;
using System.Collections.Generic;
using System.Text;

namespace IrBusWebService.Results
{
    public class GetBusServiceResult : ErrorStatusResult
    {
        public BusServiceGo BusServiceGo { get; set; }

    }
}
