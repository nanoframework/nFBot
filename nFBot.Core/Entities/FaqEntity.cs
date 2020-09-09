using System;

namespace nanoFramework.Tools.nFBot.Core.Entities
{
    public class FaqEntity
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public string Content { get; set; }
        public ulong Creator { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}