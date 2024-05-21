using Newtonsoft.Json;

namespace DreadAwakening
{
    public class UserAssetsHelper 
    {
        private HttpClient _httpClient;
        private Dictionary<int, List<GuAsset>> _userAssetDictionary;
        public UserAssetsHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _userAssetDictionary = new Dictionary<int, List<GuAsset>>();
        }
        public async Task<List<GuAsset>> GetUserAssetsAsync(int userId)
        {
            if (userId == 0) return null;

            if (_userAssetDictionary.ContainsKey(userId))
                return _userAssetDictionary[userId];


            var offset = 0;
            var limit = 7500;
            var assetRequest = $"https://assets.prod.prod.godsunchained.com/v2/asset?user_id={userId}&assetTypes=card";
            var allUserAssets = new List<GuAsset>();

            while (true)
            {
                try
                {
                    var assetsResponse = await _httpClient.GetStringAsync($"{assetRequest}&offset={offset}&limit={limit}");

                    var assets = JsonConvert.DeserializeObject<GuAssetOverview>(assetsResponse);
                    allUserAssets.AddRange(assets.GuAssets);

                    if (assets.GuAssets.Count < limit)
                    {
                        break;
                    }

                    offset += limit;

                }
                catch (Exception ex)
                {
                    
                }

            }
            await Task.Delay(1000);

            _userAssetDictionary.TryAdd(userId, allUserAssets);

            return allUserAssets;
        }
    }
}
