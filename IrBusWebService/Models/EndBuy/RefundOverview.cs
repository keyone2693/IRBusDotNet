
namespace IrBusWebService.Models.EndBuy
{
    public class RefundOverview
    {
        public long ID { get; set; }
        public int TicketPrice { get; set; }
        public int RefundAmount { get; set; }
        public string Status { get; set; }
    }
}