using Newtonsoft.Json;

public record GuAsset
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("owner")]
    public string? TokenAddress { get; set; }

    [JsonProperty("proto")]
    public int Proto { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("details")]
    public GuAssetMetaData Details { get; set; }

    [JsonProperty("contract")]
    public string? Contract { get; set; }
    
    [JsonProperty("token_id")]
    public string? TokenId { get; set; }
    [JsonProperty("image_url")]
    public string? ImageUrl{ get; set; }
    [JsonProperty("minting_status")]
    public string? MintingStatus { get; set; }
}
