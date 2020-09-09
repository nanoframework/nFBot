//
// Copyright (c) 2020 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using DSharpPlus.CommandsNext;
using nanoFramework.Tools.nFBot.Attributes;
using System.Linq;

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
