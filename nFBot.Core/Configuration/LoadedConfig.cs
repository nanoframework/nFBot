//
// Copyright (c) 2020 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using Newtonsoft.Json;

namespace nanoFramework.Tools.nFBot.Core.Configuration
{
    public class LoadedConfig
    {
        [JsonProperty("prefix", Required = Required.Always)]
        public string Prefix { get; set; }

        [JsonProperty("debug_token", Required = Required.Always)]
        public string DebugToken { get; set; }

        [JsonProperty("status_text", Required = Required.Always)]
        public string StatusText { get; set; }
        
        [JsonProperty("welcome_message", Required = Required.Always)]
        public string WelcomeMessage { get; set; }

        [JsonProperty("admin_role_id", Required = Required.Always)]
        public ulong AdminRoleId { get; set; }
        
        [JsonProperty("storage_mode", Required = Required.Always)]
        public string StorageMode { get; set; }
        
        [JsonProperty("debug_storage_connection_string", Required = Required.Always)]
        public string DebugStorageConnectionString { get; set; }
    }
}
