public record UpdateProductRequest
{
    public int Id { get; set; }
    public int? StoreId { get; set; }
    public required string UserId { get; set; }
    public required UpdateProductDTO Data { get; set; }
}
