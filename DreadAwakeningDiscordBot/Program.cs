﻿namespace DreadAwakeningDiscordBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var bot = new Bot();
            await bot.RunAsync();

            await Task.Delay(-1);
        }
    }
}