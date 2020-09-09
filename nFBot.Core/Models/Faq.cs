using System;

namespace nanoFramework.Tools.nFBot.Core.Models
{
    public class Faq
    {
        public string Tag { get; set; }
        public string Content { get; set; }
        public ulong Creator { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}