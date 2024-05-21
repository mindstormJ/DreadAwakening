namespace DreadAwakening
{
    public class DreadPoints
    {
        public string Address{ get; set; }
        public int RewardsSet { get; set; }
        public int PremiumSet { get; set; }
        public int CraftingSet { get; set; }
        public int ChaseSet { get; set; }
        public int FullSet { get; set; }
        public int DiamondCards { get; set; }

        public int Total
        {
            get
            {
                return RewardsSet + (PremiumSet * 3) + (CraftingSet * 5) + (ChaseSet * 5) + (FullSet * 6) + DiamondCards;
            }
        }
    }

    public class PointsCalculator
    {

        public PointsCalculator(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public HttpClient _httpClient { get; }

        public async Task<List<DreadPoints>> CalculateAsync(int userId)
        {
            var userAssetsHelper = new UserAssetsHelper(_httpClient);

            var userAssets = await userAssetsHelper.GetUserAssetsAsync(userId);
            var walletAssets = userAssets.Where(u => u.TokenAddress != userId.ToString() && u.TokenAddress.ToUpper() != "ALL").ToList();


            var rewardIds = new[] { 2600, 2611, 2618, 2680, 2701, 2659, 2639, 2612, 2620, 2682, 2702, 2633,
                                    2643, 2615, 2624, 2688, 2705, 2666, 2644, 2724, 2625, 2686, 2706, 2667,
                                    2645, 2617, 2626, 2687, 2707, 2673, 2649, 2601, 2623, 2692, 2711, 2662,
                                    2648, 2603, 2634, 2693, 2710, 2669, 2641, 2605, 2628, 2681, 2715, 2661 };

            var premiumIds = new[] { 2640, 2613, 2621, 2683, 2703, 2664, 2642, 2614,
                                     2622, 2684, 2704, 2665, 2646, 2616, 2627, 2685,
                                     2708, 2678, 2654, 2737, 2633, 2699, 2721, 2734,
                                     2728, 2732, 2733, 2647, 2608, 2635, 2690, 2709,
                                     2670, 2650, 2723, 2636, 2691, 2712, 2671, 2652,
                                     2610, 2637, 2694, 2714, 2672, 2731, 2653, 2606,
                                     2631, 2697, 2716, 2675 };

            var craftingIds = new[] { 2651,2609,2726,2695,2713,2674,2655,2607,2632,
                                      2696,2718,2676,2657,2602,2629,2689,2719,2679 };

            var chaseIds = new[] { 2656, 2722, 2725, 2698, 2717, 2677, 2730, 2658, 2604, 2630, 2700, 2720, 2668, 2729 };

            var allOwnedDreadCards = userAssets.Where(u => u.Details.Set.ToLower() == "dread").ToList();

            var distinctWalletAddresses = walletAssets.Select(w => w.TokenAddress).Distinct().ToList();

            var result = new List<DreadPoints>();

            foreach (var address in distinctWalletAddresses)
            {
                var cardsInThisAddress = allOwnedDreadCards.Where(a => a.TokenAddress == address).ToList();

                int numberOfCompleteRewardsSubSet = CalculateRewardsSubSet(cardsInThisAddress, rewardIds);
                int numberOfCompletePremiumSets = CalculateRewardsSubSet(cardsInThisAddress, premiumIds);
                int numberOfCraftingSets = CalculateRewardsSubSet(cardsInThisAddress, craftingIds);
                int numberOfChaseSets = CalculateRewardsSubSet(cardsInThisAddress, chaseIds);

                var allIds = new List<int>();
                allIds.AddRange(rewardIds); ;
                allIds.AddRange(premiumIds); ;
                allIds.AddRange(craftingIds); ;
                allIds.AddRange(chaseIds);

                int numberOfCompleteSets = CalculateRewardsSubSet(cardsInThisAddress, allIds.ToArray());
                int diamondPoints = CalculateDiamondPoints(cardsInThisAddress);


                //2600 - 2737
                var points = new DreadPoints
                {
                    Address = address,
                    ChaseSet = numberOfChaseSets,
                    DiamondCards = diamondPoints,
                    CraftingSet = numberOfCraftingSets,
                    FullSet = numberOfCompleteSets,
                    PremiumSet = numberOfCompletePremiumSets,
                    RewardsSet = numberOfCompleteRewardsSubSet
                };

                result.Add(points);
            }

            return result;
        }

        private int CalculateDiamondPoints(List<GuAsset> allOwnedDreadCards)
        {
            var points = 0;
            foreach (var card in allOwnedDreadCards)
            {
                if (card.Details.Quality == "1")
                {
                    switch (card.Details.Rarity.ToLower())
                    {
                        case "common":
                            points++; break;
                        case "rare":
                            points += 2; break;
                        case "epic":
                            points += 6; break;
                        case "legendary":
                            points += 12; break;
                        default: break;
                    }
                }
            }

            return points;
        }

        private int CalculateRewardsSubSet(List<GuAsset> collection, int[] requiredIds)
        {
            var cardsToUse = new List<GuAsset>();
            foreach (var item in collection)
            {
                if (requiredIds.Contains(item.Proto))
                {
                    cardsToUse.Add(item);
                }
            }

            var numberOfSubsets = 0;
            while (cardsToUse.Count > 0)
            {
                var shouldKeepProcessing = true;
                if (shouldKeepProcessing)
                {
                    var allCardsFound = false;
                    var missingCard = false;
                    foreach (var rewardId in requiredIds)
                    {
                        var firstCard = cardsToUse.FirstOrDefault(c => c.Proto == rewardId);

                        if (firstCard == null)
                        {
                            shouldKeepProcessing = false;
                            missingCard = true;
                            break;
                        }
                        cardsToUse.Remove(firstCard);
                    }

                    if (!missingCard)
                    {
                        numberOfSubsets++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            var missingCardsForAnotherSubSet = requiredIds.Where(r => !cardsToUse.Select(c => c.Proto).ToList().Contains(r)).ToList();


            return numberOfSubsets;
        }
    }
}
