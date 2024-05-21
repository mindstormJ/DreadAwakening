using Discord;
using Discord.Commands;
using DreadAwakening;



namespace DreadAwakeningDiscordBot
{
    public class DreadModule : ModuleBase<SocketCommandContext>
    {
                
        // ~say hello world -> hello world
        [Command("Collection")]
        [Summary("Echoes a message.")]
        public async Task DreadAsync([Remainder][Summary("Calculates your dread awakening set points")] string message)
        {
            var success = int.TryParse(message, out var userId);
            if (!success)
            {
                await ReplyAsync("Please only enter your GU user id.");
                return;
            }

            var calculationWarning = new EmbedBuilder()
                 .AddField("Warning", $"{userId}: Calculation of the points takes a while, certainly if you have a large collectionn")
                 .WithAuthor(Context.Client.CurrentUser)
                 .WithFooter(footer => footer.Text = "MindstormJ")
                 .WithColor(Color.Red)
                 .WithTitle("Points calculation")
                 .WithCurrentTimestamp();

            await ReplyAsync(embed: calculationWarning.Build());
           


            var httpClient = new HttpClient();
            var pointsCalculator = new PointsCalculator(httpClient);
            var pointsPerAddress = await pointsCalculator.CalculateAsync(userId);

            var walletWithZeroPoints = false;
            if (pointsPerAddress.Count == 0)
            {
                var noWallets =  new EmbedBuilder()

                .AddField("User", $"{userId}")
                .AddField("No wallets found", $"I did not find any wallets on for GU ID {userId}")
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer => footer.Text = "MindstormJ")
                .WithColor(Color.Red)
                .WithTitle("No wallets found")
                .WithCurrentTimestamp();

                await ReplyAsync(embed: noWallets.Build());
;
                return;
            }

            var embed = new EmbedBuilder()
            .AddField("User", $"{userId}")
            .WithAuthor(Context.Client.CurrentUser)
            .WithFooter(footer => footer.Text = "MindstormJ")
            .WithColor(Color.Red)
            .WithTitle("Points")
            .WithCurrentTimestamp();

            foreach (var points in pointsPerAddress)
            {
                if (points.Total == 0)
                {
                    walletWithZeroPoints = true;
                }
                else
                {
                    embed
                    .AddField($"Points for wallet {points.Address.Substring(0, 5)}: {points.Total}", $"Full rewards sets: {points.RewardsSet}. \r\nFull premium sets: {points.PremiumSet}.\r\nFull crafting sets: {points.CraftingSet}.\r\nFull chaase sets: {points.ChaseSet}. \r\nFull sets: {points.FullSet}. \r\nDiamond points: {points.DiamondCards}");
                }
            }

            await ReplyAsync(embed: embed.Build());

        }
    }
}
