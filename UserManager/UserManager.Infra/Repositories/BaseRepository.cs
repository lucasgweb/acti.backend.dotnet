using Acti.Domain.Entities;
using Acti.Domain.Repositories;
using Acti.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Acti.Infra.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : Base
{
    private readonly AppDbContext _context;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public virtual async Task<T> Add(T obj)
    {
        _context.Add(obj);
        await _context.SaveChangesAsync();

        return obj;
    }

    public virtual async Task Delete(int id)
    {
        var entity = await Get(id);

        if (entity != null)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public virtual async Task<T> Get(int id)
    {
        var entity = await _context.Set<T>()
            .AsNoTracking()
            .Where(x => x.Id == id).ToListAsync();
        return entity.FirstOrDefault();
    }

    public virtual async Task<List<T>> Get()
    {
        return await _context.Set<T>()
            .AsNoTracking()
            .ToListAsync();
    }

    public virtual async Task<T> Update(T obj)
    {
        _context.Entry(obj).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return obj;
    }
}