using IRBusDotNet.Models.EndBuy;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRBusDotNet.Results
{
    public class BuyTicketResult : ErrorStatusResult
    {
        public TicketSummary TicketSummary { get; set; }
    }
}
