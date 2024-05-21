﻿using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;

namespace DreadAwakeningDiscordBot
{
    public class DreadAwakeningCommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;


        public DreadAwakeningCommandHandler(DiscordSocketClient client, CommandService commands)
        {
            _commands = commands;
            _client = client;
        }


        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                services: null);
        }


        public async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message is null) return;

            int argPos = 0;

            if (!(message.HasCharPrefix('!', ref argPos) ||
            message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
            message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(context: context,
                argPos: argPos,
                services: null);
        }
    }
}
