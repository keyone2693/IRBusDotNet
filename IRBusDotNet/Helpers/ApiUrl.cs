using System;
using System.Collections.Generic;
using System.Text;

namespace IRBusDotNet.Helpers
{
    public static class ApiUrl
    {
        public const string BaseUrl = "https://api.safar724.net";
        private const string VersionUrl = "/api/v1.0";
        public const string SafarBaseUrl = BaseUrl + VersionUrl;

        public const string Token = "/token";

        public const string GetCities = "/cities";
        public const string GetServices = "/buses";
        public const string GetBusService = "/buses";
        public const string BuyTicket = "/tickets";
        public const string InfoBuyTicket = "/tickets";
        public const string RefundOverviewTicket = "/tickets";
        public const string RefundTicket = "/tickets";

    }
}
