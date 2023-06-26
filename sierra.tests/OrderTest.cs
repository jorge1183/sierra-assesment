using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using sierra.Controllers;
using sierra.Repository.Interfaces;
using sierra.DTO.Order;
using sierra.Repository;
using AutoMapper;
using sierra.DTO.Product;
using sierra.Model.Entities;
using sierra.DTO.Customer;
using Microsoft.AspNetCore.Mvc;

namespace sierra.tests;

public class OrderTest
{
  private IMapper _mapper;

  private ProductResponse _product1Response = 
    new () {
        Id = 1, 
        Name = "Product 1", 
        Price = 120
      };
  private CustomerResponse _customer1Response =
    new () {
      Id = 1,
      Name = "Customer 1",
    };
  private OrderResponse _order1Response =
    new () {
      Id = 1,
      Price = 120,
      Quantity = 1,
      Total = 120,
      Customer = new () {
        Id = 1,
        Name = "Customer 1",
      },
      Product = new () {
        Id = 1, 
        Name = "Product 1", 
        Price = 120
      },
    };

  [Fact]
  public void Order_ValidValues_ShouldReturnOk()
  {
    Initialize();
    var mockLogger = new Mock<ILogger<OrderController>>();
    var mockDbContext = new Mock<SierraContext>("FakeConnectionString");
    var mockCustomerRepository = 
      new Mock<GenericRepository<Customer, CustomerRequest, CustomerResponse>>(mockDbContext.Object, _mapper);
    mockCustomerRepository
      .Setup(c => c.GetById(1))
      .Returns(_customer1Response);
    
    var mockProductRepository = 
      new Mock<GenericRepository<Product, ProductRequest, ProductResponse>>(mockDbContext.Object, _mapper);
    mockProductRepository
      .Setup(c => c.GetById(1))
      .Returns(_product1Response);
    
    var mockOrderRepository = 
      new Mock<OrderRepository>(mockDbContext.Object, _mapper);
    mockOrderRepository
      .Setup(c => c.Add(It.IsAny<OrderRequest>(), It.IsAny<decimal>()))
      .Returns(_order1Response);
    
    var mockUnitOfWork = new Mock<IUnitOfWork>();
    mockUnitOfWork.Setup(u => u.CustomerRepository).Returns(mockCustomerRepository.Object);
    mockUnitOfWork.Setup(u => u.ProductRepository).Returns(mockProductRepository.Object);
    mockUnitOfWork.Setup(u => u.OrderRepository).Returns(mockOrderRepository.Object);
    var orderController = new OrderController(mockLogger.Object, mockUnitOfWork.Object);
    var result = orderController.Add(
      new OrderRequest
      {
        CustomerId = 1,
        ProductId = 1,
        Quantity = 1,
      });

    var actionResult = Assert.IsType<ActionResult<OrderResponse>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    Assert.IsType<OrderResponse>(okResult.Value);
  }

  [Theory]
  [InlineData(2, 1, 1)]
  [InlineData(1, 2, 1)]
  public void Order_InvalidCustomerProduct_ShouldReturnNotFound(int customerId, int productId, int quantity)
  {
    Initialize();
    var mockLogger = new Mock<ILogger<OrderController>>();
    var mockDbContext = new Mock<SierraContext>("FakeConnectionString");
    var mockCustomerRepository = 
      new Mock<GenericRepository<Customer, CustomerRequest, CustomerResponse>>(mockDbContext.Object, _mapper);
    mockCustomerRepository
      .Setup(c => c.GetById(1))
      .Returns(_customer1Response);
    
    var mockProductRepository = 
      new Mock<GenericRepository<Product, ProductRequest, ProductResponse>>(mockDbContext.Object, _mapper);
    mockProductRepository
      .Setup(c => c.GetById(1))
      .Returns(_product1Response);
    
    var mockOrderRepository = 
      new Mock<OrderRepository>(mockDbContext.Object, _mapper);
    mockOrderRepository
      .Setup(c => c.Add(It.IsAny<OrderRequest>(), It.IsAny<decimal>()))
      .Returns(_order1Response);
    
    var mockUnitOfWork = new Mock<IUnitOfWork>();
    mockUnitOfWork.Setup(u => u.CustomerRepository).Returns(mockCustomerRepository.Object);
    mockUnitOfWork.Setup(u => u.ProductRepository).Returns(mockProductRepository.Object);
    mockUnitOfWork.Setup(u => u.OrderRepository).Returns(mockOrderRepository.Object);
    var orderController = new OrderController(mockLogger.Object, mockUnitOfWork.Object);
    var result = orderController.Add(
      new OrderRequest
      {
        CustomerId = customerId,
        ProductId = productId,
        Quantity = quantity,
      });

    var actionResult = Assert.IsType<ActionResult<OrderResponse>>(result);
    Assert.IsType<NotFoundObjectResult>(actionResult.Result);
  }

  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  public void Order_InvalidQuantity_ShouldReturnBadRequest(int quantity)
  {
    var mockLogger = new Mock<ILogger<OrderController>>();
    var mockDbContext = new Mock<SierraContext>("FakeConnectionString");
    var mockCustomerRepository = 
      new Mock<GenericRepository<Customer, CustomerRequest, CustomerResponse>>(mockDbContext.Object, _mapper);
    mockCustomerRepository
      .Setup(c => c.GetById(1))
      .Returns(_customer1Response);
    
    var mockProductRepository = 
      new Mock<GenericRepository<Product, ProductRequest, ProductResponse>>(mockDbContext.Object, _mapper);
    mockProductRepository
      .Setup(c => c.GetById(1))
      .Returns(_product1Response);
    
    var mockOrderRepository = 
      new Mock<OrderRepository>(mockDbContext.Object, _mapper);
    mockOrderRepository
      .Setup(c => c.Add(It.IsAny<OrderRequest>(), It.IsAny<decimal>()))
      .Returns(_order1Response);
    
    var mockUnitOfWork = new Mock<IUnitOfWork>();
    mockUnitOfWork.Setup(u => u.CustomerRepository).Returns(mockCustomerRepository.Object);
    mockUnitOfWork.Setup(u => u.ProductRepository).Returns(mockProductRepository.Object);
    mockUnitOfWork.Setup(u => u.OrderRepository).Returns(mockOrderRepository.Object);
    
    var orderController = new OrderController(mockLogger.Object, mockUnitOfWork.Object);
    var result = orderController.Add(
      new OrderRequest
      {
        CustomerId = 1,
        ProductId = 1,
        Quantity = quantity,
      });
    
    var actionResult = Assert.IsType<ActionResult<OrderResponse>>(result);
    var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);

    Assert.Equal("Please enter a value equal or greater than 1", badRequestResult.Value.ToString());
  }

  private void Initialize()
  {
    var configuration = new MapperConfiguration(cfg =>
      {
        cfg.CreateMap<ProductRequest, Product>();
        cfg.CreateMap<Product, ProductResponse>();

        cfg.CreateMap<CustomerRequest, Customer>();
        cfg.CreateMap<Customer, CustomerResponse>();

        cfg.CreateMap<Order, OrderResponse>();
      }
    );
    _mapper = configuration.CreateMapper();
  }
}