using IrBusWebService.Models.EndBuy;
using System;
using System.Collections.Generic;
using System.Text;

namespace IrBusWebService.Results
{
    public class BuyTicketResult : ErrorStatusResult
    {
        public TicketSummary TicketSummary { get; set; }
    }
}
