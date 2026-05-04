public record JWtOptions
{
    public required string Key { get; set; } = string.Empty;
    public required string Issuer { get; set; } = string.Empty;
    public required string Audience { get; set; } = string.Empty;
}
