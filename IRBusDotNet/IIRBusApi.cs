using IRBusDotNet.Models.EndBuy;
using IRBusDotNet.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IRBusDotNet
{
    public interface IIRBusApi
    {
        string Getcode(string statusCode);

        BusTokenResult GetToken(string username, string password, string granttype = "password");

        bool ValidateToken(DateTime issued, DateTime expires);

        GetCitiesResult GetCities();
        GetServicesResult GetServices(string startCityId, string endCityId, string date);

        GetBusServiceResult GetBusService(string routId);

        BuyTicketResult BuyTicket(string servId, TicketToBook ticket);

        InfoBuyTicketResult InfoBuyTicket(long ticketId);
        RefundOverviewTicketResult RefundOverviewTicket(string ticketId);

        RefundTicketResult RefundTicket(string ticketId);

        Task<BusTokenResult> GetTokenAsync(string username, string password, string granttype = "password");

        Task<GetCitiesResult> GetCitiesAsync();
        Task<GetServicesResult> GetServicesAsync(string startCityId, string endCityId, string date);

        Task<GetBusServiceResult> GetBusServiceAsync(string routId);

        Task<BuyTicketResult> BuyTicketAsync(string servId, TicketToBook ticket);

        Task<InfoBuyTicketResult> InfoBuyTicketAsync(long ticketId);
        Task<RefundOverviewTicketResult> RefundOverviewTicketAsync(string ticketId);

        Task<RefundTicketResult> RefundTicketAsync(string ticketId);
    }
}
