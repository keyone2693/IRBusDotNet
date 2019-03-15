using System;

namespace IrBusWebService.Models.EndBuy.Info
{
    public class TicketInfo
    {
        public int ID { get; set; }
        public string TicketNumber { get; set; } = "ناموفق";
        public Contact Contact { get; set; }
        public string BusID { get; set; }
        public string Company { get; set; }
        public Operatingcompany OperatingCompany { get; set; }
        public Boardingpoint BoardingPoint { get; set; }
        public Droppingpoint DroppingPoint { get; set; }
        public string BusType { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime IssueDate { get; set; }
        public Passenger[] Passengers { get; set; }
        public string Status { get; set; }
    }

    public class Contact
    {
        public string Name { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
    }

    public class Operatingcompany
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class Boardingpoint
    {
        public string City { get; set; }
        public string Terminal { get; set; }
        public Additionalinfo AdditionalInfo { get; set; }
    }

    public class Additionalinfo
    {
        public City City { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
    }

    public class City
    {
        public string Name { get; set; }
        public string EnglishName { get; set; }
    }

    public class Droppingpoint
    {
        public string City { get; set; }
        public string Terminal { get; set; }
        public Additionalinfo1 AdditionalInfo { get; set; }
    }

    public class Additionalinfo1
    {
        public City1 City { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
    }

    public class City1
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
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