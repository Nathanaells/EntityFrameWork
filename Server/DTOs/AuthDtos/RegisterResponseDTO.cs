public record RegisterResponseDTO
{
    public required string Id { get; init; }

    public required string Username { get; init; }
    public required string Email { get; init; }
}
