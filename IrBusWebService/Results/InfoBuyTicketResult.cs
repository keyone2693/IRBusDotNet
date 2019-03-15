using IrBusWebService.Models.EndBuy.Info;
using System;
using System.Collections.Generic;
using System.Text;

namespace IrBusWebService.Results
{
    public class InfoBuyTicketResult : ErrorStatusResult
    {
        public TicketInfo TicketInfo { get; set; }

    }
}
