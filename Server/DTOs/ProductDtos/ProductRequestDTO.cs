public record ProductRequestDTO
{
    public int StoreId { get; set; }
    public int Id { get; set; }
    public string? UserId { get; set; } = string.Empty;
}
