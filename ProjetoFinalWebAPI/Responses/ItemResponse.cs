namespace Requests;

public class ItemResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Category { get; set; }
    public required string Variant { get; set; }
    public required string Size { get; set; }
    public required double Price { get; set; }
    public int Quantity { get; set; }
    public required string EanCode { get; set; }
}
