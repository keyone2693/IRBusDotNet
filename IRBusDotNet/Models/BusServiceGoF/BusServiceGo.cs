using System;

namespace IrBusWebService.Models.BusServiceGoF
{
    public class BusServiceGo
    {
        public string ID { get; set; }
        public string Company { get; set; }
        public Operatingcompany OperatingCompany { get; set; }
        public Boardingpoint BoardingPoint { get; set; }
        public Droppingpoint[] DroppingPoints { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
        public float CommissionPercentage { get; set; }
        public Financial Financial { get; set; }
        public DateTime DepartureDate { get; set; }
        public int AvailableSeats { get; set; }
        public string Status { get; set; }
        public Seate[] Seates { get; set; }
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
        public double MaxApplicableDiscountPercentage { get; set; }
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

    public class Seate
    {
        public int Number { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public string Status { get; set; }
    }

}