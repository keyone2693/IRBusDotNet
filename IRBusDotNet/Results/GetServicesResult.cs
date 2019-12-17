using IRBusDotNet.Models;
using System.Collections.Generic;

namespace IRBusDotNet.Results
{
    public class GetServicesResult : ErrorStatusResult
    {
        public IEnumerable<BusServices> BusServices { get; set; }
    }
}
