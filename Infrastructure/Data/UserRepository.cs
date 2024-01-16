using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ComplaintContext _complaintContext;
        public UserRepository(ComplaintContext complaintContext)
        {
            _complaintContext = complaintContext;

        }


        public async Task<User> AddUser(User user)
        {
            _complaintContext.Users.Add(user);
            await _complaintContext.SaveChangesAsync();
            return user;
        }
        
        public async Task<IReadOnlyList<User>> getAllUsers()
        {
            return await _complaintContext.Users
                .Include(p => p.Complaints)
                .AsNoTracking()
                .ToListAsync();
        }


        public User GetUserById(int userId)
        {
            return _complaintContext.Users.FirstOrDefault(u => u.UserId == userId);
        }
    }
}