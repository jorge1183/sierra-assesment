namespace sierra.Model.Entities;

public class Order: IEntity
{
  public int Id { get; set; }
  public Customer Customer { get; set; }
  public Product Product { get; set; }
  public decimal Price { get; set; }
  public int Quantity {get; set;}
  public decimal Total { get; set; }
}
