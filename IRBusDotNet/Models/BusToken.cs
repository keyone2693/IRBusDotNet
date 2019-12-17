

namespace IrBusWebService.Models
{
    public class BusToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
        public string issued { get; set; }
        public string expires { get; set; }
        public string error { get; set; } = "بدون خطا";
        public string error_description { get; set; } = "بدون توضیح";

    }
}