using System.ComponentModel.DataAnnotations;

namespace sierra.DTO.Order;
public class OrderRequest
{
  [Required]
  public int? CustomerId { get; set; }

  [Required]
  public int? ProductId { get; set; }

  [Range(1, int.MaxValue, ErrorMessage = "Please enter a value equal or greater than 1")]
  [Required]
  public int? Quantity { get; set; }
}