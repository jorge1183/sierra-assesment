using AutoMapper;
using sierra.DTO.Customer;
using sierra.DTO.Product;
using sierra.Model.Entities;
using sierra.Repository.Interfaces;

namespace sierra.Repository;

public class UnitOfWork: IDisposable, IUnitOfWork
{
  private SierraContext _context;
  private IMapper _mapper;
  private OrderRepository orderRepository;
  private GenericRepository<Product, ProductRequest, ProductResponse> productRepository;
  private GenericRepository<Customer, CustomerRequest, CustomerResponse> customerRepository;

  public UnitOfWork(SierraContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public OrderRepository OrderRepository
  {
    get
    {
      orderRepository ??= new OrderRepository(_context, _mapper);
      return orderRepository;
    }
  }

  public GenericRepository<Product, ProductRequest, ProductResponse> ProductRepository
  {
    get
    {
      productRepository ??= new GenericRepository<Product, ProductRequest, ProductResponse>(_context, _mapper);
      return productRepository;
    }
  }

  public GenericRepository<Customer, CustomerRequest, CustomerResponse> CustomerRepository
  {
    get
    {
      customerRepository ??= new GenericRepository<Customer, CustomerRequest, CustomerResponse>(_context, _mapper);
      return customerRepository;
    }
  }

  private bool disposed = false;

      protected virtual void Dispose(bool disposing)
      {
          if (!disposed)
          {
              if (disposing)
              {
                  _context.Dispose();
              } 
          }
          disposed = true;
      }

      public void Dispose()
      {
          Dispose(true);
          GC.SuppressFinalize(this);
      }
}