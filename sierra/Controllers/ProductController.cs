using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sierra.DTO.Product;
using sierra.Repository.Interfaces;

namespace sierra.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
  private readonly ILogger<ProductController> _logger;
  private readonly IUnitOfWork _unitOfWork;
  public ProductController(ILogger<ProductController> logger, IUnitOfWork unitOfWork)
  {
      _logger = logger;
      _unitOfWork = unitOfWork;
  }

  [HttpGet("{id}")]
  public IActionResult Get(int id)
  {
    _logger.LogInformation("Retrieveing product with id:{0}", id);

    var product = _unitOfWork.ProductRepository.GetById(id);
    if (product == null)
    {
      return NotFound();
    }

    _logger.LogInformation("Product retrieved successfully", id);
    return Ok(product);
  }

  [HttpGet]
  public IActionResult GetAll([FromQuery]int? skip, [FromQuery]int? count)
  {
    _logger.LogInformation("Retrieving all products");

    var products = _unitOfWork.ProductRepository.GetAll(skip, count);

    _logger.LogInformation($"Returning {products.Count} products");
    return Ok(products);
  }

  [HttpPost]
  [Authorize]
  public IActionResult Add([FromBody] ProductRequest product)
  {
    _logger.LogInformation($"Adding product");

    if (!ModelState.IsValid) {
      return BadRequest(ModelState);
    }
    
    var result = _unitOfWork.ProductRepository.Add(product);

    _logger.LogInformation($"Product added sucessfully");
    return Ok(result);
  }

  [HttpPut("{id}")]
  [Authorize]
  public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProductRequest product)
  {
    _logger.LogInformation("Updating product");

    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var exists = await _unitOfWork.ProductRepository.ExistsAsync(id);
    if (!exists)
    {
      return NotFound();
    }

    var result = await _unitOfWork.ProductRepository.UpdateAsync(id, product);

    _logger.LogInformation($"Product updated sucessfully");
    return Ok(result);
  }
}
