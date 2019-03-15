using System;


namespace IrBusWebService.Results
{
    public class BusTokenResult : ErrorStatusResult
    {
        public string AccessToken { get; set; }
        public DateTime Created { get; set; }
        public DateTime ExpireIn { get; set; }
        public string TokenType { get; set; }
        public string UserName { get; set; }

    }
}
