using Newtonsoft.Json;

public record GuAssetOverview
{

    [JsonProperty("data")]
    public List<GuAsset> GuAssets { get; set; }
}
