public record StoreResponseDTO
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Location { get; init; }
}
