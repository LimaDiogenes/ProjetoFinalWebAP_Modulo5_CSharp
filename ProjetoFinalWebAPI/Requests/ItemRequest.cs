using System.ComponentModel.DataAnnotations;

namespace Requests;

public class BaseItemRequest
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Category { get; set; }
    public string? Variant { get; set; }
    public string? Size { get; set; }    
    public double Price { get; set; }
    public int Quantity { get; set; }
    public string? EanCode { get; set; }
}

public class ToItemResponse : BaseItemRequest
{
    
}
