using Acti.Domain.Entities;

namespace Acti.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetByEmail(string email);
}