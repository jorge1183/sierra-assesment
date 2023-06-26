using sierra.Model.Entities;

namespace sierra.Repository.Interfaces;

public interface IOrderRepository
{
  Order Add(Order order, int price);
  Order GetById(int id);
  IList<Order> GetAll(int? skip, int? count);
}