using IrBusWebService.Models;
using IrBusWebService.Models.BusServiceGoF;
using IrBusWebService.Models.EndBuy;
using IrBusWebService.Models.EndBuy.Info;
using IrBusWebService.Results;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IrBusWebService
{
    public class BusApi : IBusApi
    {
        private string _token;
        public BusApi()
        {
        }
        public BusApi(string token)
        {
            _token = token;
        }
        public string Getcode(string statusCode)
        {
            switch (statusCode)
            {
                case "Pending":
                    return "بلیط در وضعیت انتظار قرار دارد";
                case "Issued":
                    return "بلیط شما صادر شده است";
                case "Refunded":
                    return "بلیط شما پس داده شده است";
                case "Failed":
                    return "بلیط شما ناموفق میباشد";
                case "PaymentUnsuccessful":
                    return "پرداخت بلیط ناموفق میباشد";
                case "Canceled":
                    return "بلیط شما کنسل شده است";
                case "Rejected":
                    return "بلیط شما رد شده است";
                case "Refundable":
                    return "بلیط شما قابل پس دادن میباشد";
                case "Nonrefundable":
                    return "بلیط شما قابل پس دادن نمیباشد";
                case "BadArgument":
                    return "";
                case "InvalidTicketStatus":
                    return "وضعیت بلیط مشخص نیست";
                case "RefundRequestIsRejected":
                    return "کنسلی بلیط رد شد";
                case "RefundDateExpired":
                    return "زمان کنسلی گذشته است";
                case "SeatUnavailable":
                    return "صندلی غیر قابل خرید";
                case "InvalidBankAccountNumber":
                    return "مشکل بانکی";
                case "InvalidBusServiceStatus":
                    return "مشکل در وضعیت اتبوبس";
                case "InsufficientCredit":
                    return "اعتبار ناکافی";
                case "InvalidDiscount":
                    return "تخفیف نامعتبر";
            }
            return "خطا";
        }

        public BusTokenResult GetToken(string username, string password, string granttype = "password")
        {
            var client = new RestClient("https://api.safar724.com/token");

            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", "grant_type=" + granttype + "&username=" + username + "&password=" + password + "&undefined=", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
            {
                var tokencontent = JsonConvert.DeserializeObject<BusToken>(response.Content.Replace(".expires", "expires").Replace(".issued", "issued"));

                CultureInfo provider = CultureInfo.InvariantCulture;
                DateTime issued = DateTime.ParseExact(tokencontent.issued.Split(',')[1].TrimEnd(new char[] { 'T', 'M', 'G' }).Trim(), "dd MMM yyyy HH:mm:ss", provider);
                DateTime expires = DateTime.ParseExact(tokencontent.expires.Split(',')[1].TrimEnd(new char[] { 'T', 'M', 'G' }).Trim(), "dd MMM yyyy HH:mm:ss", provider);
                var result = new BusTokenResult
                {
                    Created = issued,
                    ExpireIn = expires,
                    AccessToken = tokencontent.access_token,
                    TokenType = tokencontent.token_type,
                    UserName = tokencontent.userName,
                    Status = true,
                    Error = ""
                };
                return result;
            }
            else
            {
                var tokencontent = JsonConvert.DeserializeObject<BusToken>(response.Content);
                return new BusTokenResult
                {
                    Status = false,
                    Error = tokencontent.error,
                    ErrorDescription = tokencontent.error_description
                };
            }
        }
        public bool ValidateToken(DateTime issued, DateTime expires)
        {
            if (expires > issued)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public GetCitiesResult GetCities()
        {
            var client = new RestClient("https://api.safar724.com/api/v1.0/cities");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + _token);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
            {
                var cities = JsonConvert.DeserializeObject<IEnumerable<BusCity>>(response.Content);

                return new GetCitiesResult
                {
                    BusCities = cities,
                    Status = true
                };
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new GetCitiesResult
                {
                    Status = false,
                    Error = "اعتبار سنجی نشد",
                    ErrorDescription = "باید دوباره توکن را تولید کنید"
                };
            }
            else
            {
                return new GetCitiesResult
                {
                    Status = false,
                    Error = "خطای نامشخص",
                    ErrorDescription = response.ErrorMessage
                };
            }
        }
        public GetServicesResult GetServices( string startCityId, string endCityId, string date)
        {
            var client = new RestClient("https://api.safar724.com/api/v1.0/buses/" + startCityId + "/" + endCityId + "/" + date);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + _token);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
            {
                var busservice = JsonConvert.DeserializeObject<IEnumerable<BusServices>>(response.Content);
                return new GetServicesResult
                {
                    BusServices = busservice,
                    Status = true
                };
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new GetServicesResult
                {
                    Status = false,
                    Error = "اعتبار سنجی نشد",
                    ErrorDescription = "باید دوباره توکن را تولید کنید"
                };
            }
            else
            {
                return new GetServicesResult
                {
                    Status = false,
                    Error = "خطای نامشخص",
                    ErrorDescription = response.ErrorMessage
                };
            }
        }

        public GetBusServiceResult GetBusService( string routId)
        {
            var client = new RestClient("https://api.safar724.com/api/v1.0/buses/" + routId);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + _token);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
            {
                var busservice = JsonConvert.DeserializeObject<BusServiceGo>(response.Content);
                return new GetBusServiceResult
                {
                    BusServiceGo = busservice,
                    Status = true
                };
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new GetBusServiceResult
                {
                    Status = false,
                    Error = "اعتبار سنجی نشد",
                    ErrorDescription = "باید دوباره توکن را تولید کنید"
                };
            }
            else
            {
                return new GetBusServiceResult
                {
                    Status = false,
                    Error = "خطای نامشخص",
                    ErrorDescription = response.ErrorMessage
                };
            }
        }

        public BuyTicketResult BuyTicket( string servId, TicketToBook ticket)
        {
            var client = new RestClient("https://api.safar724.com/api/v1.0/tickets");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + _token);
            request.AddHeader("Content-type", "application/json");
            request.AddJsonBody(ticket);
            IRestResponse response = client.Execute(request);
            if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
            {
                var busservice = JsonConvert.DeserializeObject<TicketSummary>(response.Content);
                return new BuyTicketResult
                {
                    TicketSummary = busservice,
                    Status = true
                };

            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new BuyTicketResult
                {
                    Status = false,
                    Error = "اعتبار سنجی نشد",
                    ErrorDescription = "باید دوباره توکن را تولید کنید"
                };
            }
            else
            {
                return new BuyTicketResult
                {
                    Status = false,
                    Error = "خطای نامشخص",
                    ErrorDescription = response.ErrorMessage
                };
            }
        }

        public InfoBuyTicketResult InfoBuyTicket(long ticketId)
        {
            var client = new RestClient("https://api.safar724.com/api/v1.0/tickets/" + ticketId);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + _token);
            IRestResponse response = client.Execute(request);
            if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
            {
                var busservice = JsonConvert.DeserializeObject<TicketInfo>(response.Content);
                return new InfoBuyTicketResult
                {
                    TicketInfo = busservice,
                    Status = true
                };
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new InfoBuyTicketResult
                {
                    Status = false,
                    Error = "اعتبار سنجی نشد",
                    ErrorDescription = "باید دوباره توکن را تولید کنید"
                };
            }
            else
            {
                return new InfoBuyTicketResult
                {
                    Status = false,
                    Error = "خطای نامشخص",
                    ErrorDescription = response.ErrorMessage
                };
            }
        }
        public RefundOverviewTicketResult RefundOverviewTicket(string ticketId)
        {
            var client = new RestClient("https://api.safar724.com/api/v1.0/tickets/" + ticketId + "/refund");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + _token);
            IRestResponse response = client.Execute(request);
            if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
            {
                var busservice = JsonConvert.DeserializeObject<RefundOverview>(response.Content);
                return new RefundOverviewTicketResult
                {
                    RefundOverview = busservice,
                    Status = true
                };
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new RefundOverviewTicketResult
                {
                    Status = false,
                    Error = "اعتبار سنجی نشد",
                    ErrorDescription = "باید دوباره توکن را تولید کنید"
                };
            }
            else
            {
                return new RefundOverviewTicketResult
                {
                    Status = false,
                    Error = "خطای نامشخص",
                    ErrorDescription = response.ErrorMessage
                };
            }
        }

        public RefundTicketResult RefundTicket(string ticketId)
        {
            var client = new RestClient("https://api.safar724.com/api/v1.0/tickets/" + ticketId + "/refund");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + _token);
            IRestResponse response = client.Execute(request);
            if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
            {
                return new RefundTicketResult
                {
                    Status = true
                };
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new RefundTicketResult
                {
                    Status = false,
                    Error = "اعتبار سنجی نشد",
                    ErrorDescription = "باید دوباره توکن را تولید کنید"
                };
            }
            else
            {
                return new RefundTicketResult
                {
                    Status = false,
                    Error = "خطای نامشخص",
                    ErrorDescription = response.ErrorMessage
                };
            }
        }




        public BusTokenResult GetTokenAsync(string username, string password, string granttype = "password")
        {
            var client = new RestClient("https://api.safar724.com/token");

            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", "grant_type=" + granttype + "&username=" + username + "&password=" + password + "&undefined=", ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);
            BusTokenResult result = null;
            var asyncHandle = client.ExecuteAsync(request, response =>
            {
                if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                {
                    var tokencontent = JsonConvert.DeserializeObject<BusToken>(response.Content.Replace(".expires", "expires").Replace(".issued", "issued"));

                    CultureInfo provider = CultureInfo.InvariantCulture;
                    DateTime issued = DateTime.ParseExact(tokencontent.issued.Split(',')[1].TrimEnd(new char[] { 'T', 'M', 'G' }).Trim(), "dd MMM yyyy HH:mm:ss", provider);
                    DateTime expires = DateTime.ParseExact(tokencontent.expires.Split(',')[1].TrimEnd(new char[] { 'T', 'M', 'G' }).Trim(), "dd MMM yyyy HH:mm:ss", provider);
                    result = new BusTokenResult
                    {
                        Created = issued,
                        ExpireIn = expires,
                        AccessToken = tokencontent.access_token,
                        TokenType = tokencontent.token_type,
                        UserName = tokencontent.userName,
                        Status = true,
                        Error = ""
                    };

                }
                else
                {
                    var tokencontent = JsonConvert.DeserializeObject<BusToken>(response.Content);
                    result = new BusTokenResult
                    {
                        Status = false,
                        Error = tokencontent.error,
                        ErrorDescription = tokencontent.error_description
                    };
                }


            });
            asyncHandle.Abort();
            return result;
            
        }

        public GetCitiesResult GetCitiesAsync()
        {
            var client = new RestClient("https://api.safar724.com/api/v1.0/cities");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + _token);
            // IRestResponse response = client.Execute(request);
            GetCitiesResult result = null;
            var asyncHandle = client.ExecuteAsync(request, response =>
            {
                if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                {
                    var cities = JsonConvert.DeserializeObject<IEnumerable<BusCity>>(response.Content);

                    result = new GetCitiesResult
                    {
                        BusCities = cities,
                        Status = true
                    };
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result = new GetCitiesResult
                    {
                        Status = false,
                        Error = "اعتبار سنجی نشد",
                        ErrorDescription = "باید دوباره توکن را تولید کنید"
                    };
                }
                else
                {
                    result = new GetCitiesResult
                    {
                        Status = false,
                        Error = "خطای نامشخص",
                        ErrorDescription = response.ErrorMessage
                    };
                }
            });
            asyncHandle.Abort();
            return result;

        }
        public GetServicesResult GetServicesAsync( string startCityId, string endCityId, string date)
        {
            var client = new RestClient("https://api.safar724.com/api/v1.0/buses/" + startCityId + "/" + endCityId + "/" + date);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + _token);
            //IRestResponse response = client.Execute(request);
            GetServicesResult result = null;
            var asyncHandle = client.ExecuteAsync(request, response =>
            {
                if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                {
                    var busservice = JsonConvert.DeserializeObject<IEnumerable<BusServices>>(response.Content);
                    result = new GetServicesResult
                    {
                        BusServices = busservice,
                        Status = true
                    };
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result = new GetServicesResult
                    {
                        Status = false,
                        Error = "اعتبار سنجی نشد",
                        ErrorDescription = "باید دوباره توکن را تولید کنید"
                    };
                }
                else
                {
                    result = new GetServicesResult
                    {
                        Status = false,
                        Error = "خطای نامشخص",
                        ErrorDescription = response.ErrorMessage
                    };
                }
            });
            asyncHandle.Abort();
            return result;

        }

        public GetBusServiceResult GetBusServiceAsync(string routId)
        {
            var client = new RestClient("https://api.safar724.com/api/v1.0/buses/" + routId);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + _token);
            // IRestResponse response = client.Execute(request);
            GetBusServiceResult result = null;
            var asyncHandle = client.ExecuteAsync(request, response =>
            {
                if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                {
                    var busservice = JsonConvert.DeserializeObject<BusServiceGo>(response.Content);
                    result = new GetBusServiceResult
                    {
                        BusServiceGo = busservice,
                        Status = true
                    };
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result = new GetBusServiceResult
                    {
                        Status = false,
                        Error = "اعتبار سنجی نشد",
                        ErrorDescription = "باید دوباره توکن را تولید کنید"
                    };
                }
                else
                {
                    result = new GetBusServiceResult
                    {
                        Status = false,
                        Error = "خطای نامشخص",
                        ErrorDescription = response.ErrorMessage
                    };
                }
            });
            asyncHandle.Abort();
            return result;

        }

        public BuyTicketResult BuyTicketAsync(string servId, TicketToBook ticket)
        {
            var client = new RestClient("https://api.safar724.com/api/v1.0/tickets");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + _token);
            request.AddHeader("Content-type", "application/json");
            request.AddJsonBody(ticket);
            //IRestResponse response = client.Execute(request);
            BuyTicketResult result = null;
            var asyncHandle = client.ExecuteAsync(request, response =>
            {
                if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                {
                    var busservice = JsonConvert.DeserializeObject<TicketSummary>(response.Content);
                    result = new BuyTicketResult
                    {
                        TicketSummary = busservice,
                        Status = true
                    };

                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result = new BuyTicketResult
                    {
                        Status = false,
                        Error = "اعتبار سنجی نشد",
                        ErrorDescription = "باید دوباره توکن را تولید کنید"
                    };
                }
                else
                {
                    result = new BuyTicketResult
                    {
                        Status = false,
                        Error = "خطای نامشخص",
                        ErrorDescription = response.ErrorMessage
                    };
                }
            });
            asyncHandle.Abort();
            return result;

        }

        public InfoBuyTicketResult InfoBuyTicketAsync(long ticketId)
        {
            var client = new RestClient("https://api.safar724.com/api/v1.0/tickets/" + ticketId);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + _token);
            //IRestResponse response = client.Execute(request);
            InfoBuyTicketResult result = null;
            var asyncHandle = client.ExecuteAsync(request, response =>
            {
                if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                {
                    var busservice = JsonConvert.DeserializeObject<TicketInfo>(response.Content);
                    result = new InfoBuyTicketResult
                    {
                        TicketInfo = busservice,
                        Status = true
                    };
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result = new InfoBuyTicketResult
                    {
                        Status = false,
                        Error = "اعتبار سنجی نشد",
                        ErrorDescription = "باید دوباره توکن را تولید کنید"
                    };
                }
                else
                {
                    result = new InfoBuyTicketResult
                    {
                        Status = false,
                        Error = "خطای نامشخص",
                        ErrorDescription = response.ErrorMessage
                    };
                }
            });
            asyncHandle.Abort();
            return result;

        }
        public RefundOverviewTicketResult RefundOverviewTicketAsync(string ticketId)
        {
            var client = new RestClient("https://api.safar724.com/api/v1.0/tickets/" + ticketId + "/refund");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + _token);
            //IRestResponse response = client.Execute(request);
            RefundOverviewTicketResult result = null;
            var asyncHandle = client.ExecuteAsync(request, response =>
            {
                if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                {
                    var busservice = JsonConvert.DeserializeObject<RefundOverview>(response.Content);
                    result = new RefundOverviewTicketResult
                    {
                        RefundOverview = busservice,
                        Status = true
                    };
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result = new RefundOverviewTicketResult
                    {
                        Status = false,
                        Error = "اعتبار سنجی نشد",
                        ErrorDescription = "باید دوباره توکن را تولید کنید"
                    };
                }
                else
                {
                    result = new RefundOverviewTicketResult
                    {
                        Status = false,
                        Error = "خطای نامشخص",
                        ErrorDescription = response.ErrorMessage
                    };
                }
            });
            asyncHandle.Abort();
            return result;

        }

        public RefundTicketResult RefundTicketAsync(string ticketId)
        {
            var client = new RestClient("https://api.safar724.com/api/v1.0/tickets/" + ticketId + "/refund");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + _token);
            // IRestResponse response = client.Execute(request);
            RefundTicketResult result = null;
            var asyncHandle = client.ExecuteAsync(request, response =>
            {
                if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                {
                    result = new RefundTicketResult
                    {
                        Status = true
                    };
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result = new RefundTicketResult
                    {
                        Status = false,
                        Error = "اعتبار سنجی نشد",
                        ErrorDescription = "باید دوباره توکن را تولید کنید"
                    };
                }
                else
                {
                    result = new RefundTicketResult
                    {
                        Status = false,
                        Error = "خطای نامشخص",
                        ErrorDescription = response.ErrorMessage
                    };
                }
            });
            asyncHandle.Abort();
            return result;
        }
    }
}
