using System;

namespace nFAQ.Attributes
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
