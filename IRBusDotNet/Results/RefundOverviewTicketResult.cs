using IRBusDotNet.Models.EndBuy;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRBusDotNet.Results
{
    public class RefundOverviewTicketResult : ErrorStatusResult
    {
        public RefundOverview RefundOverview { get; set; }
    }
}
