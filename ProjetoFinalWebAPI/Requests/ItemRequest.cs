namespace Requests;

public class BaseItemRequest
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Category { get; set; }
    public string? Variant { get; set; }
    public string? Size { get; set; }
    public double Price { get; set; }
}

public class ToItemResponse : BaseItemRequest
{
    
}
