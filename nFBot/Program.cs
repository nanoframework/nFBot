using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using nFBot.Handlers;
using nFBot.Jobs;
using nFBot.Modules;

namespace nFBot
{
    class Program
    {
        private static DiscordClient _discord;
        private static CommandsNextExtension _commands;
        private static Config _config;

        private static readonly ServiceCollection ServiceCollection = new ServiceCollection();

        static void Main(string[] args)
        {
            try
            {
                _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("./config.json"));
            }
            catch
            {
                Console.WriteLine("Unable to load config from ./config.json");
                Console.WriteLine();
                Console.WriteLine("Ensure the config exists and is valid. Use the config.example.json for reference");
                Console.WriteLine();
                Console.WriteLine("Press enter to continue");

                Console.ReadLine();

                Environment.Exit(1);
            }

            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            ServiceCollection.AddSingleton(_config);

            _discord = new DiscordClient(new DiscordConfiguration
            {
                Token = _config.Token,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            _commands = _discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new List<string>
                {
                    _config.Prefix
                },
                EnableMentionPrefix = true,
                Services = ServiceCollection.BuildServiceProvider(),
                IgnoreExtraArguments = true,
                EnableDefaultHelp = false
            });

            _commands.CommandErrored += CommandErroredUsageHandler.Handle;

            _commands.RegisterCommands<AdminModule>();
            //_commands.RegisterCommands<HelpModule>();
            _commands.RegisterCommands<FaqModule>();

            _discord.Ready += InitialStart;

            await _discord.ConnectAsync();

            await Task.Delay(-1);
        }

        private static readonly AsyncEventHandler<ReadyEventArgs> InitialStart = async e =>
        {
            e.Client.Ready -= InitialStart;

            new Thread(async () => { await StatusJob.JobTask(_config, _discord); }).Start();
        };
    }
}
