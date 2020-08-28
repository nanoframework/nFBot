namespace nFBot.Core.Configuration
{
    public class FinalConfig
    {
        public string Prefix { get; set; }
        public string Token { get; set; }
        public string StatusText { get; set; }
        public ulong AdminRoleId { get; set; }
        public string StorageMode { get; set; }
        public string StorageConnectionString { get; set; }
    }
}