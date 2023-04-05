using Acti.Domain.Entities;
using Acti.Domain.Repositories;
using Acti.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Acti.Infra.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User> GetByEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
        return user;
    }
}