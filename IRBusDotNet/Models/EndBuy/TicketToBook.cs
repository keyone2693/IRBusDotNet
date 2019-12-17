
namespace IrBusWebService.Models.EndBuy
{
    public class TicketToBook
    {
        public string BusID { get; set; }
        public string PaymentMethod { get; set; }
        public float DesiredDiscountPercentage { get; set; }
        public Passenger[] Passengers { get; set; }
        public Contact Contact { get; set; }
        public string HookUrl { get; set; }
    }

    public class Contact
    {
        public string Name { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
    }

    public class Passenger
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SeatNumber { get; set; }
        public string NationalCode { get; set; }
        public string Gender { get; set; }
    }

}