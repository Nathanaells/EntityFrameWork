public record ProductDTO
{
    public int Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public int StoreId { get; init; }
}
