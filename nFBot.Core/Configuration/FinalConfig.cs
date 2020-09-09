//
// Copyright (c) 2020 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Tools.nFBot.Core.Configuration
{
    public class FinalConfig
    {
        public string Prefix { get; set; }
        public string Token { get; set; }
        public string StatusText { get; set; }
        public string WelcomeMessage { get; set; }
        public ulong AdminRoleId { get; set; }
        public string StorageMode { get; set; }
        public string StorageConnectionString { get; set; }
    }
}