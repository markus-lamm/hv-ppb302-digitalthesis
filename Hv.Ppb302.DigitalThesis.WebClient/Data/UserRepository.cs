﻿using Hv.Ppb302.DigitalThesis.WebClient.Models;

namespace Hv.Ppb302.DigitalThesis.WebClient.Data;

public class UserRepository(DigitalThesisDbContext dbContext)
{
    public User? Get(Guid id)
    {
        try
        {
            return dbContext.Users.FirstOrDefault(g => g.Id == id);
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public User? GetByUsername(string username)
    {
        try
        {
            return dbContext.Users.FirstOrDefault(g => g.Username == username);
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
            return dbContext.Users.FirstOrDefault(g => g.Username == username && g.Password == password);
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
            return dbContext.Users.ToList();
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
            var existingUser = dbContext.Users.FirstOrDefault(m => m.Username == user.Username);
            if (existingUser != null)
            {
                throw new Exception("A user with the same username already exists");
            }
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }

    public void Update(Profile profile)
    {
        try
        {
            var existingUser = dbContext.Users.FirstOrDefault(m => m.Id == profile.Id);
            if (existingUser == null)
            {
                throw new Exception("The user does not exist");
            }

            // The username is occupied by another user
            var occupiedName = dbContext.Users.FirstOrDefault(m => m.Username == profile.Username && profile.Username != existingUser.Username);
            if (occupiedName?.Username != null)
            {
                throw new Exception("A user with the same username already exists");
            }

            // Only updates fields that are not empty
            if (!string.IsNullOrWhiteSpace(profile.Username))
            {
                existingUser.Username = profile.Username;
            }
            if (!string.IsNullOrWhiteSpace(profile.NewPassword))
            {
                existingUser.Password = profile.NewPassword;
            }

            dbContext.SaveChanges();
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
            var existingUser = dbContext.Users.Find(id);
            if (existingUser == null)
            {
                throw new Exception("The user does not exist");
            }
            dbContext.Users.Remove(existingUser);
            dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Internal Server Error");
        }
    }
}
