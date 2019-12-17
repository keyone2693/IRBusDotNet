using IRBusDotNet.Models.BusServiceGoF;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRBusDotNet.Results
{
    public class GetBusServiceResult : ErrorStatusResult
    {
        public BusServiceGo BusServiceGo { get; set; }

    }
}
