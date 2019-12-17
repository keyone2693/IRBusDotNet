using IRBusDotNet;
using System;

namespace ExampleTest
{
    class Program
    {
        static async void Main(string[] args)
        {
            IIRBusApi api = new IRBusApi();        
            var token = api.GetToken("Username","Password");

            bool flag = api.ValidateToken(token.Created,token.ExpireIn);

            //IBusApi api = new BusApi("token");

            IIRBusApi _api = new IRBusApi(token.AccessToken);
            var cities = _api.GetCities();


            var result1 = api.GetCities();
            var result2 = await api.GetCitiesAsync();
            Console.WriteLine(result1.Status + "-----" + result1.Error + "-----" + result1.ErrorDescription);
            Console.WriteLine(result2.Status + "-----" + result2.Error + "-----" + result2.ErrorDescription);
            Console.ReadLine();
        }
    }
}
