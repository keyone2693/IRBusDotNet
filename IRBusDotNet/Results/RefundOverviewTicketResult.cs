using IrBusWebService.Models.EndBuy;
using System;
using System.Collections.Generic;
using System.Text;

namespace IrBusWebService.Results
{
    public class RefundOverviewTicketResult : ErrorStatusResult
    {
        public RefundOverview RefundOverview { get; set; }
    }
}
