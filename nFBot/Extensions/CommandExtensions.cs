using System.Linq;
using DSharpPlus.CommandsNext;
using nFBot.Attributes;

namespace nFBot.Extensions
{
    public static class CommandExtensions
    {
        public static string GetUsage(this Command command)
        {
            UsageAttribute usage = (UsageAttribute)command.CustomAttributes.FirstOrDefault(a => a.GetType() == typeof(UsageAttribute));
            return usage != null ? usage.Value : "";
        }
    }
}
