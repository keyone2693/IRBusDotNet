using IrBusWebService;
using System;

namespace ExampleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IBusApi api = new BusApi("token");
            var result1 = api.GetCities();
            var result2 = api.GetCitiesAsync();
            Console.WriteLine(result1.Status + "-----" + result1.Error + "-----" + result1.ErrorDescription);
            Console.WriteLine(result2.Status + "-----" + result2.Error + "-----" + result2.ErrorDescription);
            Console.ReadLine();
        }
    }
}
