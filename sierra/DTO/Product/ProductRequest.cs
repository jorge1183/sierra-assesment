using System.ComponentModel.DataAnnotations;

namespace sierra.DTO.Product;
public class ProductRequest
{
  [Required]
  [MaxLength(20)]
  public string Name { get; set;}

  [Required]
  [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a decimal number greater than {1}")]
  public decimal Price { get; set;}
}