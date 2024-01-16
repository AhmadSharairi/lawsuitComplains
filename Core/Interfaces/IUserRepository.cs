using Core.Entities;

namespace Core.Interfaces
{
  public interface IUserRepository
  {
    User GetUserById(int userId);
    public Task<User> AddUser(User user);
    public Task<IReadOnlyList<User>> getAllUsers();
  }
}