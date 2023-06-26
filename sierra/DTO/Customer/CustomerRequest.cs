using System.ComponentModel.DataAnnotations;

namespace sierra.DTO.Customer;

public class CustomerRequest
{
  [Required]
  [MaxLength(20)]
  public string Name { get; set; }
}