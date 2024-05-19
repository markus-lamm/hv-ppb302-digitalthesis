using Hv.Ppb302.DigitalThesis.WebClient.Models;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class UserRepository : IRepository<User>
{
    private readonly DigitalThesisDbContext _dbContext;

    public UserRepository(DigitalThesisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public User? Get(Guid id)
    {
        return _dbContext.Users.FirstOrDefault(g => g.Id == id);
    }

    public User? GetByCredentials(string username, string password)
    {
        return _dbContext.Users.FirstOrDefault(g => g.Username == username && g.Password == password);
    }

    public List<User>? GetAll()
    {
        return _dbContext.Users.ToList();
    }

    public void Create(User user)
    {
        var existingUser = _dbContext.Users.FirstOrDefault(m => m.Username == user.Username);
        if (existingUser != null)
        {
            throw new Exception("A user with the same username already exists");
        }
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }

    public void Update(User user)
    {
        var existingUser = _dbContext.Users.FirstOrDefault( m => m.Username == user.Username);
        if (existingUser == null)
        {
            throw new Exception("The user does not exist");
        }
        existingUser.Username = user.Username;
        existingUser.Password = user.Password;
        _dbContext.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var existingUser = _dbContext.Users.Find(id);
        if (existingUser == null)
        {
            throw new Exception("The molecular mosaic does not exist");
        }
        _dbContext.Users.Remove(existingUser);
        _dbContext.SaveChanges();
    }
}
