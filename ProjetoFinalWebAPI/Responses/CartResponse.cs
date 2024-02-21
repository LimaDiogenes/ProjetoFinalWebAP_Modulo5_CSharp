using System.Collections.Generic;

namespace Requests;

public class CartResponse
{
    public int Id { get; set; }
    public List<ItemResponse>? ItemsList {  get; set; }
}
