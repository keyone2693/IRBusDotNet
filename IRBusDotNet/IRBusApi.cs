using IRBusDotNet.Helpers;
using IRBusDotNet.Models;
using IRBusDotNet.Models.BusServiceGoF;
using IRBusDotNet.Models.EndBuy;
using IRBusDotNet.Models.EndBuy.Info;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IRBusDotNet
{
    public class IRBusApi : IIRBusApi
    {
        private readonly HttpClient _http;
        private StringContent _content;
        private HttpResponseMessage _response;

        private string _token = "";

        public IRBusApi()
        {
            _http = new HttpClient();
        }
        public void AddToken(string token)
        {
            _token = token;
        }

        #region Synchronous

        public BusServiceResult<BusTokenResult> GetToken(string username, string password, string granttype = "password")
        {
            var result = new BusServiceResult<BusTokenResult>();

            _http.DefaultRequestHeaders.Clear();

            _content = new StringContent(
           JsonConvert.SerializeObject(new { username, password, granttype }), UTF8Encoding.UTF8, "application/x-www-form-urlencoded");

            _response = _http.PostAsync(ApiUrl.BaseUrl.ToUrl(ApiUrl.Token), _content).Result;


            if (_response.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<BusToken>(_response.Content.ReadAsStringAsync().Result);

                CultureInfo provider = CultureInfo.InvariantCulture;
                DateTime issued = DateTime.ParseExact(res.issued.Split(',')[1].TrimEnd(new char[] { 'T', 'M', 'G' }).Trim(), "dd MMM yyyy HH:mm:ss", provider);
                DateTime expires = DateTime.ParseExact(res.expires.Split(',')[1].TrimEnd(new char[] { 'T', 'M', 'G' }).Trim(), "dd MMM yyyy HH:mm:ss", provider);
                var tokenResult = new BusTokenResult
                {
                    Created = issued,
                    ExpireIn = expires,
                    AccessToken = res.access_token,
                    TokenType = res.token_type,
                    UserName = res.userName,
                };
                _token = res.access_token;
                result.Result = tokenResult;
                result.Status = true;
            }
            else
            {
                var res = JsonConvert.DeserializeObject<BusErrorResult>(_response.Content.ReadAsStringAsync().Result);
                result.Message = res.ToError();
                result.Status = false;
            }
            return result;

        }
        public BusServiceResult<bool> ValidateToken(DateTime issued, DateTime expires)
        {
            var result = new BusServiceResult<bool>();
            result.Status = true;

            if (expires > issued)
            {
                result.Result = true;
            }
            else
            {
                result.Result = false;
            }
            return result;

        }

        public BusServiceResult<List<BusCity>> GetCities()
        {
            var result = new BusServiceResult<List<BusCity>>();

            _http.DefaultRequestHeaders.Clear();

            _http.DefaultRequestHeaders.Add("Authorization", Constants.PreToken + _token);
            _response = _http.GetAsync(ApiUrl.SafarBaseUrl.ToUrl(ApiUrl.GetCities)).Result;

            if (_response.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<List<BusCity>>(_response.Content.ReadAsStringAsync().Result);
                result.Result = res;
                result.Status = true;
            }
            else if (_response.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Message = "خطا در اعتبار سنجی دوباره توکن را تولید کنید";
                result.Status = false;
                result.Unauthorized = true;
            }
            else
            {
                var res = JsonConvert.DeserializeObject<BusErrorResult>(_response.Content.ReadAsStringAsync().Result);
                result.Message = res.ToError();
                result.Status = false;
            }

            return result;
        }
        public BusServiceResult<List<BusServices>> GetServices(string startCityId, string endCityId, string date)
        {
            var result = new BusServiceResult<List<BusServices>>();
            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("Authorization", Constants.PreToken + _token);

            _response = _http.GetAsync(ApiUrl.SafarBaseUrl.ToUrl(ApiUrl.GetServices + "/" + startCityId + "/" + endCityId + "/" + date)).Result;


            if (_response.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<List<BusServices>>(_response.Content.ReadAsStringAsync().Result);

                result.Result = res;
                result.Status = true;
            }
            else if (_response.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Message = "خطا در اعتبار سنجی دوباره توکن را تولید کنید";
                result.Status = false;
                result.Unauthorized = true;
            }
            else
            {
                var res = JsonConvert.DeserializeObject<BusErrorResult>(_response.Content.ReadAsStringAsync().Result);
                result.Message = res.ToError();
                result.Status = false;
            }
            return result;
        }

        public BusServiceResult<BusServiceGo> GetBusService(string routId)
        {
            var result = new BusServiceResult<BusServiceGo>();
            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("Authorization", Constants.PreToken + _token);

            _response = _http.GetAsync(ApiUrl.SafarBaseUrl.ToUrl(ApiUrl.GetBusService + "/" + routId)).Result;

            if (_response.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<BusServiceGo>(_response.Content.ReadAsStringAsync().Result);

                result.Result = res;
                result.Status = true;
            }
            else if (_response.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Message = "خطا در اعتبار سنجی دوباره توکن را تولید کنید";
                result.Status = false;
                result.Unauthorized = true;
            }
            else
            {
                var res = JsonConvert.DeserializeObject<BusErrorResult>(_response.Content.ReadAsStringAsync().Result);
                result.Message = res.ToError();
                result.Status = false;
            }

            return result;

        }

        public BusServiceResult<TicketSummary> BuyTicket(string servId, TicketToBook ticket)
        {
            var result = new BusServiceResult<TicketSummary>();
            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("Authorization", Constants.PreToken + _token);

            _content = new StringContent(
                       JsonConvert.SerializeObject(ticket), UTF8Encoding.UTF8, "application/json");
            _response = _http.PostAsync(ApiUrl.SafarBaseUrl.ToUrl(ApiUrl.BuyTicket), _content).Result;

            if (_response.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<TicketSummary>(_response.Content.ReadAsStringAsync().Result);
                result.Result = res;
                result.Status = true;

            }
            else if (_response.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Message = "خطا در اعتبار سنجی دوباره توکن را تولید کنید";
                result.Status = false;
                result.Unauthorized = true;
            }
            else
            {
                var res = JsonConvert.DeserializeObject<BusErrorResult>(_response.Content.ReadAsStringAsync().Result);
                result.Message = res.ToError();
                result.Status = false;
            }
            return result;

        }

        public BusServiceResult<TicketInfo> InfoBuyTicket(long ticketId)
        {
            var result = new BusServiceResult<TicketInfo>();
            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("Authorization", Constants.PreToken + _token);

            _response = _http.GetAsync(ApiUrl.SafarBaseUrl.ToUrl(ApiUrl.InfoBuyTicket + "/" + ticketId)).Result;

            if (_response.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<TicketInfo>(_response.Content.ReadAsStringAsync().Result);

                result.Result = res;
                result.Status = true;
            }
            else if (_response.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Message = "خطا در اعتبار سنجی دوباره توکن را تولید کنید";
                result.Status = false;
                result.Unauthorized = true;
            }
            else
            {
                var res = JsonConvert.DeserializeObject<BusErrorResult>(_response.Content.ReadAsStringAsync().Result);
                result.Message = res.ToError();
                result.Status = false;
            }
            return result;

        }
        public BusServiceResult<RefundOverview> RefundOverviewTicket(string ticketId)
        {
            var result = new BusServiceResult<RefundOverview>();
            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("Authorization", Constants.PreToken + _token);

            _response = _http.GetAsync(ApiUrl.SafarBaseUrl.ToUrl(ApiUrl.RefundOverviewTicket + "/" + ticketId + "/refund")).Result;

            if (_response.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<RefundOverview>(_response.Content.ReadAsStringAsync().Result);

                result.Result = res;
                result.Status = true;
            }
            else if (_response.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Message = "خطا در اعتبار سنجی دوباره توکن را تولید کنید";
                result.Status = false;
                result.Unauthorized = true;
            }
            else
            {
                var res = JsonConvert.DeserializeObject<BusErrorResult>(_response.Content.ReadAsStringAsync().Result);
                result.Message = res.ToError();
                result.Status = false;
            }
            return result;

        }

        public BusServiceResult<bool> RefundTicket(string ticketId)
        {
            var result = new BusServiceResult<bool>();
            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("Authorization", Constants.PreToken + _token);

            _response = _http.PostAsync(ApiUrl.SafarBaseUrl.ToUrl(ApiUrl.RefundTicket + "/" + ticketId + "/refund"), null).Result;

            if (_response.IsSuccessStatusCode)
            {
                result.Status = true;
            }
            else if (_response.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Message = "خطا در اعتبار سنجی دوباره توکن را تولید کنید";
                result.Status = false;
                result.Unauthorized = true;
            }
            else
            {
                var res = JsonConvert.DeserializeObject<BusErrorResult>(_response.Content.ReadAsStringAsync().Result);
                result.Message = res.ToError();
                result.Status = false;
            }
            return result;

        }
        #endregion
        #region Asynchronous 
        public async Task<BusServiceResult<BusTokenResult>> GetTokenAsync(string username, string password, string granttype = "password")
        {
            var result = new BusServiceResult<BusTokenResult>();

            _http.DefaultRequestHeaders.Clear();

            _content = new StringContent(
           JsonConvert.SerializeObject(new { username, password, granttype }), UTF8Encoding.UTF8, "application/x-www-form-urlencoded");

            _response =await _http.PostAsync(ApiUrl.BaseUrl.ToUrl(ApiUrl.Token), _content);


            if (_response.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<BusToken>(await _response.Content.ReadAsStringAsync());

                CultureInfo provider = CultureInfo.InvariantCulture;
                DateTime issued = DateTime.ParseExact(res.issued.Split(',')[1].TrimEnd(new char[] { 'T', 'M', 'G' }).Trim(), "dd MMM yyyy HH:mm:ss", provider);
                DateTime expires = DateTime.ParseExact(res.expires.Split(',')[1].TrimEnd(new char[] { 'T', 'M', 'G' }).Trim(), "dd MMM yyyy HH:mm:ss", provider);
                var tokenResult = new BusTokenResult
                {
                    Created = issued,
                    ExpireIn = expires,
                    AccessToken = res.access_token,
                    TokenType = res.token_type,
                    UserName = res.userName,
                };
                _token = res.access_token;
                result.Result = tokenResult;
                result.Status = true;
            }
            else
            {
                var res = JsonConvert.DeserializeObject<BusErrorResult>(await _response.Content.ReadAsStringAsync());
                result.Message = res.ToError();
                result.Status = false;
            }
            return result;

        }
        public async Task<BusServiceResult<bool>> ValidateTokenAsync(DateTime issued, DateTime expires)
        {
            var result = new BusServiceResult<bool>();
            result.Status = true;

            if (expires > issued)
            {
                result.Status = true;
            }
            else
            {
                result.Status = false;
            }
            return result;

        }

        public async Task<BusServiceResult<List<BusCity>>> GetCitiesAsync()
        {
            var result = new BusServiceResult<List<BusCity>>();

            _http.DefaultRequestHeaders.Clear();

            _http.DefaultRequestHeaders.Add("Authorization", Constants.PreToken + _token);
            _response = await _http.GetAsync(ApiUrl.SafarBaseUrl.ToUrl(ApiUrl.GetCities));

            if (_response.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<List<BusCity>>(await _response.Content.ReadAsStringAsync());
                result.Result = res;
                result.Status = true;
            }
            else if (_response.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Message = "خطا در اعتبار سنجی دوباره توکن را تولید کنید";
                result.Status = false;
                result.Unauthorized = true;
            }
            else
            {
                var res = JsonConvert.DeserializeObject<BusErrorResult>(await _response.Content.ReadAsStringAsync());
                result.Message = res.ToError();
                result.Status = false;
            }

            return result;
        }
        public async Task<BusServiceResult<List<BusServices>>> GetServicesAsync(string startCityId, string endCityId, string date)
        {
            var result = new BusServiceResult<List<BusServices>>();
            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("Authorization", Constants.PreToken + _token);

            _response = await _http.GetAsync(ApiUrl.SafarBaseUrl.ToUrl(ApiUrl.GetServices + "/" + startCityId + "/" + endCityId + "/" + date));


            if (_response.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<List<BusServices>>(await _response.Content.ReadAsStringAsync());

                result.Result = res;
                result.Status = true;
            }
            else if (_response.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Message = "خطا در اعتبار سنجی دوباره توکن را تولید کنید";
                result.Status = false;
                result.Unauthorized = true;
            }
            else
            {
                var res = JsonConvert.DeserializeObject<BusErrorResult>(await _response.Content.ReadAsStringAsync());
                result.Message = res.ToError();
                result.Status = false;
            }
            return result;
        }

        public async Task<BusServiceResult<BusServiceGo>>GetBusServiceAsync(string routId)
        {
            var result = new BusServiceResult<BusServiceGo>();
            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("Authorization", Constants.PreToken + _token);

            _response = await _http.GetAsync(ApiUrl.SafarBaseUrl.ToUrl(ApiUrl.GetBusService + "/" + routId));

            if (_response.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<BusServiceGo>(await _response.Content.ReadAsStringAsync());

                result.Result = res;
                result.Status = true;
            }
            else if (_response.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Message = "خطا در اعتبار سنجی دوباره توکن را تولید کنید";
                result.Status = false;
                result.Unauthorized = true;
            }
            else
            {
                var res = JsonConvert.DeserializeObject<BusErrorResult>(await _response.Content.ReadAsStringAsync());
                result.Message = res.ToError();
                result.Status = false;
            }

            return result;

        }

        public async Task<BusServiceResult<TicketSummary>> BuyTicketAsync(string servId, TicketToBook ticket)
        {
            var result = new BusServiceResult<TicketSummary>();
            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("Authorization", Constants.PreToken + _token);

            _content = new StringContent(
                       JsonConvert.SerializeObject(ticket), UTF8Encoding.UTF8, "application/json");
            _response = await _http.PostAsync(ApiUrl.SafarBaseUrl.ToUrl(ApiUrl.BuyTicket), _content);

            if (_response.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<TicketSummary>(await _response.Content.ReadAsStringAsync());
                result.Result = res;
                result.Status = true;

            }
            else if (_response.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Message = "خطا در اعتبار سنجی دوباره توکن را تولید کنید";
                result.Status = false;
                result.Unauthorized = true;
            }
            else
            {
                var res = JsonConvert.DeserializeObject<BusErrorResult>(await _response.Content.ReadAsStringAsync());
                result.Message = res.ToError();
                result.Status = false;
            }
            return result;

        }

        public async Task<BusServiceResult<TicketInfo>> InfoBuyTicketAsync(long ticketId)
        {
            var result = new BusServiceResult<TicketInfo>();
            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("Authorization", Constants.PreToken + _token);

            _response = await _http.GetAsync(ApiUrl.SafarBaseUrl.ToUrl(ApiUrl.InfoBuyTicket + "/" + ticketId));

            if (_response.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<TicketInfo>(await _response.Content.ReadAsStringAsync());

                result.Result = res;
                result.Status = true;
            }
            else if (_response.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Message = "خطا در اعتبار سنجی دوباره توکن را تولید کنید";
                result.Status = false;
                result.Unauthorized = true;
            }
            else
            {
                var res = JsonConvert.DeserializeObject<BusErrorResult>(await _response.Content.ReadAsStringAsync());
                result.Message = res.ToError();
                result.Status = false;
            }
            return result;

        }
        public async Task<BusServiceResult<RefundOverview>> RefundOverviewTicketAsync(string ticketId)
        {
            var result = new BusServiceResult<RefundOverview>();
            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("Authorization", Constants.PreToken + _token);

            _response = await _http.GetAsync(ApiUrl.SafarBaseUrl.ToUrl(ApiUrl.RefundOverviewTicket + "/" + ticketId + "/refund"));

            if (_response.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<RefundOverview>(await _response.Content.ReadAsStringAsync());

                result.Result = res;
                result.Status = true;
            }
            else if (_response.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Message = "خطا در اعتبار سنجی دوباره توکن را تولید کنید";
                result.Status = false;
                result.Unauthorized = true;
            }
            else
            {
                var res = JsonConvert.DeserializeObject<BusErrorResult>(await _response.Content.ReadAsStringAsync());
                result.Message = res.ToError();
                result.Status = false;
            }
            return result;

        }

        public async Task<BusServiceResult<bool>> RefundTicketAsync(string ticketId)
        {
            var result = new BusServiceResult<bool>();
            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("Authorization", Constants.PreToken + _token);

            _response = await _http.PostAsync(ApiUrl.SafarBaseUrl.ToUrl(ApiUrl.RefundTicket + "/" + ticketId + "/refund"), null);

            if (_response.IsSuccessStatusCode)
            {
                result.Status = true;
            }
            else if (_response.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Message = "خطا در اعتبار سنجی دوباره توکن را تولید کنید";
                result.Status = false;
                result.Unauthorized = true;
            }
            else
            {
                var res = JsonConvert.DeserializeObject<BusErrorResult>(await _response.Content.ReadAsStringAsync());
                result.Message = res.ToError();
                result.Status = false;
            }
            return result;

        }
        #endregion



        #region dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _http.Dispose();
                    if (_content != null)
                        _content.Dispose();
                    if (_response != null)
                        _response.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~IRBusApi()
        {
            Dispose(true);
        }
        #endregion
    }
}
