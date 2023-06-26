using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sierra.Model.Entities;

namespace sierra.Repository;

public class GenericRepository<TEntity, TDTORequest, TDTOResponse> 
  where TEntity : class, IEntity
  where TDTORequest : class
  where TDTOResponse : class
{
  protected readonly SierraContext _context;
  protected readonly DbSet<TEntity> _entitySet;
  protected readonly IMapper _mapper;
  public GenericRepository(SierraContext context, IMapper mapper)
  {
    _context = context;
    _entitySet = context.Set<TEntity>();
    _mapper = mapper;
  }

  public virtual TDTOResponse Add(TDTORequest request)
  {
    var newItem = _mapper.Map<TEntity>(request);
    var addedItem = _entitySet.Add(newItem);
    _context.SaveChanges();
    return _mapper.Map<TDTOResponse>(addedItem.Entity);
  }

  public virtual async Task<TDTOResponse> UpdateAsync(int id, TDTORequest request)
  {
    var item = _mapper.Map<TEntity>(request);
    item.Id = id;
    var updatedItem = _entitySet.Update(item);
    await _context.SaveChangesAsync();
    return _mapper.Map<TDTOResponse>(updatedItem.Entity);
  }

  public virtual async Task<bool> ExistsAsync(int id) => await _entitySet.AnyAsync(e => e.Id == id);

  public virtual TDTOResponse GetById(int id)
  {
    var foundItem = _entitySet.Find(id);
    return _mapper.Map<TDTOResponse>(foundItem);
  }

  public virtual IList<TDTOResponse> GetAll(int? skip, int? count)
  {
    IQueryable<TEntity> query = GetQuery();

    if (skip.HasValue && skip.Value > 0)
    {
      query = query.Skip(skip.Value);
    }

    if (count.HasValue && count.Value > 0)
    {
      query = query.Take(count.Value);
    }

    return _mapper.Map<TDTOResponse[]>(query.ToList());
  }

  protected virtual IQueryable<TEntity> GetQuery()
  {
    return _entitySet;
  }
}
