public class Store
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Location { get; set; }
    public required string UserId { get; set; }
    public User? User { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
