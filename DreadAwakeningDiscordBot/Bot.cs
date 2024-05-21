using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Configuration;
using System.Globalization;

namespace DreadAwakeningDiscordBot
{
    public class Bot
    {
        private readonly DiscordSocketClient? _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly DreadAwakeningCommandHandler _handler;

        public Bot()
        {

            try
            {
                var config = new DiscordSocketConfig
                {
                    GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
                };

                _client = new DiscordSocketClient(config);

                _commands = new CommandService(new CommandServiceConfig
                {
                    // Again, log level:
                    LogLevel = LogSeverity.Info,

                    // There's a few more properties you can set,
                    // for example, case-insensitive commands.
                    CaseSensitiveCommands = false,
                });
                _handler = new DreadAwakeningCommandHandler(_client, _commands);


            }
            catch (Exception ex)
            {
                throw ex;  
            }
           

        }

        // If any services require the client, or the CommandService, or something else you keep on hand,
        // pass them as parameters into this method as needed.
        // If this method is getting pretty long, you can seperate it out into another file using partials.
      

        public async Task RunAsync()
        {
            
            try
            {
                await _handler.InstallCommandsAsync();

                var token = ConfigurationManager.AppSettings["BotKey"];

                await _client.LoginAsync(TokenType.Bot, token);
                await _client.StartAsync();

            }
            catch(Exception e)
            {
                throw e;
            }
                
            await Task.Delay(-1);

        }
    }
}
