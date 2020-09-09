using System;

namespace nanoFramework.Tools.nFBot.Attributes
{
    public class UsageAttribute : Attribute
    {
        public string Value { get; }

        public UsageAttribute(string value)
        {
            Value = value;
        }
    }
}
