using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sierra.DTO.Customer;
using sierra.Repository.Interfaces;

namespace sierra.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CustomerController : ControllerBase
{
  private readonly ILogger<CustomerController> _logger;
  private readonly IUnitOfWork _unitOfWork;
  public CustomerController(ILogger<CustomerController> logger, IUnitOfWork unitOfWork)
  {
      _logger = logger;
      _unitOfWork = unitOfWork;
  }

  [HttpGet("{id}")]
  public IActionResult Get(int id)
  {
    _logger.LogInformation("Retrieveing customer with id:{0}", id);

    var customer = _unitOfWork.CustomerRepository.GetById(id);
    if (customer == null)
    {
      return NotFound();
    }

    _logger.LogInformation("Customer retrieved successfully", id);
    return Ok(customer);
  }

  [HttpGet]
  public IActionResult GetAll([FromQuery]int? skip, [FromQuery]int? count)
  {
    _logger.LogInformation("Retrieving all customers");

    var customers = _unitOfWork.CustomerRepository.GetAll(skip, count);

    _logger.LogInformation($"Returning {customers.Count} customers");
    return Ok(customers);
  }

  [HttpPost]
  [Authorize]
  public IActionResult Add([FromBody] CustomerRequest customer)
  {
    _logger.LogInformation("Adding customer");
    
    if (!ModelState.IsValid) {
      return BadRequest(ModelState);
    }
    
    var result = _unitOfWork.CustomerRepository.Add(customer);

    _logger.LogInformation($"Customer added sucessfully");
    return Ok(result);
  }

  [HttpPut("{id}")]
  [Authorize]
  public async Task<IActionResult> UpdateAsync(int id, [FromBody] CustomerRequest customer)
  {
    _logger.LogInformation("Updating customer");
        
    if(!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var exists = await _unitOfWork.CustomerRepository.ExistsAsync(id);
    if (!exists)
    {
      return NotFound();
    }

    var result = await _unitOfWork.CustomerRepository.UpdateAsync(id, customer);

    _logger.LogInformation($"Customer updated sucessfully");
    return Ok(result);
  }
}
