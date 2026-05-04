public record UpdateStoreDTO
{
    public required int Id { get; set; }
    public string? Name { get; init; }
    public string? Location { get; init; }
}
