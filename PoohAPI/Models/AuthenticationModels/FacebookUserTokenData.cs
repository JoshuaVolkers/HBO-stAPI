using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Models.AuthenticationModels
{
    public class FacebookUserTokenData
    {
        [JsonProperty("app_id")]
        public long AppId { get; set; }
        public string Type { get; set; }
        public string Application { get; set; }
        [JsonProperty("expires_at")]
        public long ExpiresAt { get; set; }
        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }
        [JsonProperty("user_id")]
        public long UserId { get; set; }
    }
}
