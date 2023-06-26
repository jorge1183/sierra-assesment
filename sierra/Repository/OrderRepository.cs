using System.Data;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using sierra.DTO.Order;
using sierra.Model.Entities;

namespace sierra.Repository;

public class OrderRepository: GenericRepository<Order, OrderRequest, OrderResponse>
{
  public OrderRepository(SierraContext context, IMapper mapper) : base(context, mapper)
  {
  }

  public override IList<OrderResponse> GetAll(int? skip, int? count)
  {
    var orders = _entitySet
      .Include(x => x.Product)
      .Include(x => x.Customer);

    return _mapper.Map<OrderResponse[]>(orders.ToList());
  }

  public override OrderResponse GetById(int id)
  {
    var order = _entitySet
      .Include(x => x.Product)
      .Include(x => x.Customer)
      .FirstOrDefault(x => x.Id == id);

    return _mapper.Map<OrderResponse>(order);
  }

  public override OrderResponse Add(OrderRequest order)
  {
    throw new NotImplementedException();
  }

  public virtual OrderResponse Add(OrderRequest order, decimal price)
  {
    using var connection = _context.Database.GetDbConnection();
    using var command = connection.CreateCommand();
    command.CommandText = "AddOrder";
    command.CommandType = CommandType.StoredProcedure;
    command.Parameters.Add(new SqlParameter("customerId", order.CustomerId));
    command.Parameters.Add(new SqlParameter("productId", order.ProductId));
    command.Parameters.Add(new SqlParameter("price", price));
    command.Parameters.Add(new SqlParameter("quantity", order.Quantity));
    connection.Open();
    var id = (int)command.ExecuteScalar();
    connection.Close();
    return GetById(id);
  }

  protected override IQueryable<Order> GetQuery()
  {
    return _entitySet.Include(x => x.Product).Include(x => x.Customer);
  }
}
