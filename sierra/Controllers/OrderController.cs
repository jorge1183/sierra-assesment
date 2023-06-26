using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sierra.DTO.Order;
using sierra.Repository.Interfaces;

namespace sierra.Controllers;

[ApiController]
[Route("api/[controller]")]

public class OrderController : ControllerBase
{
  private readonly ILogger<OrderController> _logger;
  private readonly IUnitOfWork _unitOfWork;
  public OrderController(ILogger<OrderController> logger, IUnitOfWork unitOfWork)
  {
      _logger = logger;
      _unitOfWork = unitOfWork;
  }

  [HttpGet("{id}")]
  public IActionResult Get(int id)
  {
    _logger.LogInformation("Retrieveing order with id:{0}", id);

    var order = _unitOfWork.OrderRepository.GetById(id);
    if (order == null)
    {
      return NotFound();
    }

    _logger.LogInformation("Order retrieved successfully", id);
    return Ok(order);
  }

  [HttpGet]
  public IActionResult GetAll([FromQuery]int? skip, [FromQuery]int? count)
  {
    _logger.LogInformation("Retrieving all orders");

    var orders = _unitOfWork.OrderRepository.GetAll(skip, count);

    _logger.LogInformation($"Returning {orders.Count} orders");
    return Ok(orders);
  }

  [HttpPost]
  [Authorize]
  public ActionResult<OrderResponse> Add([FromBody] OrderRequest order)
  {
    _logger.LogInformation($"Adding order");

    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var customer = _unitOfWork.CustomerRepository.GetById(order.CustomerId.Value);
    if (customer == null)
    {
      return NotFound($"Customer with id {order.CustomerId} does not exist");
    }

    var product = _unitOfWork.ProductRepository.GetById(order.ProductId.Value);
    if (product == null)
    {
      return NotFound($"Product with id {order.ProductId} does not exist");
    }

    if (order.Quantity.Value <= 0) {
      return BadRequest("Please enter a value equal or greater than 1");
    }

    var result = _unitOfWork.OrderRepository.Add(order, product.Price);

    _logger.LogInformation($"Order added sucessfully");
    return Ok(result);
  }
}
