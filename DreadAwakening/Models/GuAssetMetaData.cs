using Newtonsoft.Json;

public record GuAssetMetaData
{

    [JsonProperty("effect")]
    public string Effect { get; set; }
    
    [JsonProperty("god")]
    public string God { get; set; }

    [JsonProperty("mana")]
    public int Mana { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("quality")]
    public string Quality { get; set; }

    [JsonProperty("quality_name")]
    public string QualityName { get; set; }

    [JsonProperty("rarity")]
    public string Rarity { get; set; }

    [JsonProperty("set")]
    public string Set { get; set; }
}