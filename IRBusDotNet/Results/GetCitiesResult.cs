using IRBusDotNet.Models;
using System.Collections.Generic;

namespace IRBusDotNet.Results
{
    public class GetCitiesResult : ErrorStatusResult
    {
        public IEnumerable<BusCity> BusCities { get; set; }
    }
}
