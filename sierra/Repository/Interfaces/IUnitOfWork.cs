using sierra.DTO.Customer;
using sierra.DTO.Product;
using sierra.Model.Entities;

namespace sierra.Repository.Interfaces;

public interface IUnitOfWork
{
  OrderRepository OrderRepository { get; }
  
  GenericRepository<Product, ProductRequest, ProductResponse> ProductRepository { get; }
  
  GenericRepository<Customer, CustomerRequest, CustomerResponse> CustomerRepository { get; }

}
