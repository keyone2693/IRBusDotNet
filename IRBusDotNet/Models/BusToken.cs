

using Newtonsoft.Json;

namespace IRBusDotNet.Models
{
    public class BusToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
        [JsonProperty(PropertyName = ".issued")]
        public string issued { get; set; }
        [JsonProperty(PropertyName = ".expires")]
        public string expires { get; set; }

    }
}