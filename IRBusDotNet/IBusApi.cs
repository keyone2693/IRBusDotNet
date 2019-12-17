using IrBusWebService.Models.EndBuy;
using IrBusWebService.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IrBusWebService
{
    public interface IBusApi
    {
        string Getcode(string statusCode);

        BusTokenResult GetToken(string username, string password, string granttype = "password");

        bool ValidateToken(DateTime issued, DateTime expires);

        GetCitiesResult GetCities();
        GetServicesResult GetServices( string startCityId, string endCityId, string date);

        GetBusServiceResult GetBusService( string routId);

        BuyTicketResult BuyTicket( string servId, TicketToBook ticket);

        InfoBuyTicketResult InfoBuyTicket( long ticketId);
        RefundOverviewTicketResult RefundOverviewTicket( string ticketId);

        RefundTicketResult RefundTicket( string ticketId);

        BusTokenResult GetTokenAsync(string username, string password, string granttype = "password");

        GetCitiesResult GetCitiesAsync();
        GetServicesResult GetServicesAsync( string startCityId, string endCityId, string date);

        GetBusServiceResult GetBusServiceAsync( string routId);

        BuyTicketResult BuyTicketAsync( string servId, TicketToBook ticket);

        InfoBuyTicketResult InfoBuyTicketAsync( long ticketId);
        RefundOverviewTicketResult RefundOverviewTicketAsync( string ticketId);

        RefundTicketResult RefundTicketAsync( string ticketId);
    }
}
