using System;


namespace IrBusWebService.Models
{
    public class BusServices
    {
        public string ID { get; set; }
        public string Company { get; set; }
        public Operatingcompany OperatingCompany { get; set; }
        public Boardingpoint BoardingPoint { get; set; }
        public Droppingpoint[] DroppingPoints { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
        public Financial Financial { get; set; }
        public DateTime DepartureDate { get; set; }
        public int AvailableSeats { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
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
        public string ID { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
    }

    public class Financial
    {
        public int Price { get; set; }
        public float MaxApplicableDiscountPercentage { get; set; }
        public string[] AvailablePaymentMethods { get; set; }
        public float CommissionPercentage { get; set; }
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

}
