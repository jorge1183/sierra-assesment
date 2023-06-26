using sierra.DTO.Customer;
using sierra.DTO.Product;

namespace sierra.DTO.Order;
public class OrderResponse
{
  public int Id { get; set;}
  public CustomerResponse Customer { get; set; }
  public ProductResponse Product { get; set; }
  public decimal Price { get; set; }
  public int Quantity { get; set; }
  public decimal Total { get; set; }
  
}