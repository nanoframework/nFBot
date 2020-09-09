//
// Copyright (c) 2020 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using nanoFramework.Tools.nFBot.Core.Configuration;
using nanoFramework.Tools.nFBot.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace nanoFramework.Tools.nFBot.Modules
{
    public class AdminModule : BaseCommandModule
    {
        private readonly IServiceProvider _services;
        private readonly FinalConfig _config;

        public AdminModule(IServiceProvider services, FinalConfig config)
        {
            _services = services;
            _config = config;
        }

        [Command("stop")]
        public async Task Stop(CommandContext ctx)
        {
            if (ctx.Member.Roles.All(r => r.Id != _config.AdminRoleId))
            {
                await ctx.ErrorNoPermission();
                return;
            }

            await ctx.RespondAsync("Stopping...");
            await ctx.Client.DisconnectAsync();
            Environment.Exit(0);
        }
        
        /*[Command("dbmigrate")]
        public async Task DbMigrate(CommandContext ctx, [RemainingText] string extra)
        {
            if (ctx.Member.Roles.All(r => r.Id != _config.AdminRoleId))
            {
                await ctx.ErrorNoPermission();
                return;
            }

            if (extra != "Confirm")
            {
                await ctx.RespondAsync("WARNING: Running migrations will apply any changes pending for this version of the bot. Ensure you have backed up before proceeding! This process is irreversible, run `dbmigrate Confirm` to proceed.");
                return;
            }

            switch (_config.StorageMode)
            {
                case "mysql":
                case "mssql":
                    await using (FaqDbContext context = _services.GetService(typeof(FaqDbContext)) as FaqDbContext)
                    {
                        
                        if (context == null)
                        {
                            await ctx.RespondAsync("Unable to retrieve DBContext, please ensure the correct mode has been configured");
                            return;
                        }
                        
                        await ctx.RespondAsync("Database migration in progress...");

                        try
                        {
                            await context.Database.MigrateAsync();
                        }
                        catch (Exception e)
                        {
                            ctx.Client.DebugLogger.LogMessage(LogLevel.Critical, $"{nameof(AdminModule)}.{nameof(DbMigrate)}", "Database migration crashed", DateTime.Now, e);
                            
                            await ctx.RespondAsync("Migration crashed! It may be necessary to manually roll back the backup.");
                            await ctx.RespondAsync($"```csharp\n{e}\n```");
                            
                            return;
                        }
                        

                        await ctx.RespondAsync("Database migration has completed!");
                    }

                    break;
                
                default:
                    await ctx.RespondAsync("A database migration is not applicable for the configured storage mode");
                    break;
            }
        }*/
    }
}
