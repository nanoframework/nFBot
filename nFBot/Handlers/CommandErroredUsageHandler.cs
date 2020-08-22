using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using nFBot.Exceptions;
using nFBot.Extensions;

namespace nFBot.Handlers
{
    public static class CommandErroredUsageHandler
    {
        public static async Task Handle(CommandErrorEventArgs args)
        {
            if (args.Exception.GetType().IsAssignableFrom(typeof(InvalidUsageException)) || args.Exception.Message == "Could not find a suitable overload for the command.")
            {
                string usage = args.Command.GetUsage();

                if (usage != "")
                {
                    await args.Context.RespondAsync($"Usage: `{usage}`");
                }
                else
                {
                    args.Context.Client.DebugLogger.LogMessage(LogLevel.Info, "CommandsNext", $"{args.Command.Name} was used incorrectly, but has no usage defined to help the user", DateTime.Now);
                }
            }
            else
            {
                args.Context.Client.DebugLogger.LogMessage(LogLevel.Critical, "CommandsNext", "ErrorOccured", DateTime.Now, args.Exception);
            }
        }
    }
}
