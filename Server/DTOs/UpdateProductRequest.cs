

public record UpdateProductRequest
{
    public int ProductId { get; set; }
    public int StoreId { get; set; }
    public required String UserId { get; set; }
    public required UpdateProductDTO Data { get; set; }

}