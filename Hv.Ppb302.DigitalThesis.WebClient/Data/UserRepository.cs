using Hv.Ppb302.DigitalThesis.WebClient.Models;

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
        try
        {
            return _dbContext.Users.FirstOrDefault(g => g.Id == id);
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public User? GetByCredentials(string username, string password)
    {
        try
        {
            return _dbContext.Users.FirstOrDefault(g => g.Username == username && g.Password == password);
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public List<User>? GetAll()
    {
        try
        {
            return _dbContext.Users.ToList();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Create(User user)
    {
        try
        {
            var existingUser = _dbContext.Users.FirstOrDefault(m => m.Username == user.Username);
            if (existingUser != null)
            {
                throw new Exception("A user with the same username already exists");
            }
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Update(User user)
    {
        try
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
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Delete(Guid id)
    {
        try
        {
            var existingUser = _dbContext.Users.Find(id);
            if (existingUser == null)
            {
                throw new Exception("The user does not exist");
            }
            _dbContext.Users.Remove(existingUser);
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }
}
