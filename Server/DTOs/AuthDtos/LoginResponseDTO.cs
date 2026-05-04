public record LoginResponseDTO
{
    public required string Token { get; set; }
    public required string UserId { get; set; }
    public required string Username { get; set; }
}
