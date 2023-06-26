using System.Text.Json;

namespace sierra.DTO.ErrorDetail;

public class ErrorDetails
{
  public int StatusCode { get; set; }
  public string Message { get; set; }
  public override string ToString()
  {
      return JsonSerializer.Serialize(this);
  }
}