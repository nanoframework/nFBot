using System.Linq;
using DSharpPlus.CommandsNext;
using nanoFramework.Tools.nFBot.Attributes;

namespace nanoFramework.Tools.nFBot.Extensions
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
