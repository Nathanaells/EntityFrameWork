public record ProductCreateDTO
{
    public required string Name { get; init; }
    public required decimal Price { get; init; }
}