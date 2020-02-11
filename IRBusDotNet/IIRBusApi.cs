using IRBusDotNet.Models;
using IRBusDotNet.Models.BusServiceGoF;
using IRBusDotNet.Models.EndBuy;
using IRBusDotNet.Models.EndBuy.Info;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IRBusDotNet
{
    public interface IIRBusApi : IDisposable
    {
        void AddToken(string token);

        BusServiceResult<BusTokenResult> GetToken(string username, string password, string granttype = "password");

        BusServiceResult<bool> ValidateToken(DateTime issued, DateTime expires);

        BusServiceResult<List<BusCity>> GetCities();
        BusServiceResult<List<BusServices>> GetServices(string startCityId, string endCityId, string date);

        BusServiceResult<BusServiceGo> GetBusService(string routId);

        BusServiceResult<TicketSummary> BuyTicket(string servId, TicketToBook ticket);

        BusServiceResult<TicketInfo> InfoBuyTicket(long ticketId);
        BusServiceResult<RefundOverview> RefundOverviewTicket(string ticketId);

        BusServiceResult<bool> RefundTicket(string ticketId);

        Task<BusServiceResult<BusTokenResult>> GetTokenAsync(string username, string password, string granttype = "password");

        Task<BusServiceResult<List<BusCity>>> GetCitiesAsync();
        Task<BusServiceResult<List<BusServices>>> GetServicesAsync(string startCityId, string endCityId, string date);

        Task<BusServiceResult<BusServiceGo>> GetBusServiceAsync(string routId);

        Task<BusServiceResult<TicketSummary>> BuyTicketAsync(string servId, TicketToBook ticket);

        Task<BusServiceResult<TicketInfo>> InfoBuyTicketAsync(long ticketId);
        Task<BusServiceResult<RefundOverview>> RefundOverviewTicketAsync(string ticketId);

        Task<BusServiceResult<bool>> RefundTicketAsync(string ticketId);
    }
}
