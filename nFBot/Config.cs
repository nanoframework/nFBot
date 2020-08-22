using Newtonsoft.Json;

namespace nFBot
{
    public class Config
    {
        [JsonProperty("prefix", Required = Required.Always)]
        public string Prefix { get; set; }

        [JsonProperty("token", Required = Required.Always)]
        public string Token { get; set; }

        [JsonProperty("status_text", Required = Required.Always)]
        public string StatusText { get; set; }

        [JsonProperty("admin_role_id", Required = Required.Always)]
        public ulong AdminRoleId { get; set; }
    }
}
