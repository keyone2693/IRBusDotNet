using IRBusDotNet;
using System;

namespace ExampleTest
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            IIRBusApi _api = new IRBusApi();        
            var tokenRes = _api.GetToken("username", "password");
            if (tokenRes.Status)
            {
                //var flag = api.ValidateToken(tokenRes.Result.Created, tokenRes.Result.ExpireIn);
                _api.AddToken(tokenRes.Result.AccessToken);

                var resultCities = _api.GetCities();
                Console.WriteLine(resultCities.Status + "-----" + resultCities.Message);
                Console.WriteLine(resultCities.Result);
                Console.ReadLine();
            }
            else{
                Console.WriteLine(tokenRes.Message);
                Console.ReadLine();

            }

        }
    }
}
