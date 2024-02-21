using Entities;
using Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Validators;
using Requests;
using Responses;
using Services;

namespace MockDB
{
    public interface IUserRepo
    {
        List<User> ListUsers();        
        User? GetById(int id);
        User? FindByEmail(string email);
        User CreateUser(User user);
        User UpdateUser(User user);
        bool DeleteUser(int id);
    }

    public class UsersRepo : IUserRepo
    {
        public static List<User> UsersList = new List<User>();
        public static string JsonPath = $"{Directory.GetCurrentDirectory()}\\MockDB\\Assets\\users.json";

        static UsersRepo()
        {
            UsersList = JsonIO.ReadJson<User>(JsonPath);
        }

        public User CreateUser(User user)
        {
            UsersList.Add(user);
            JsonIO.WriteJson(JsonPath, UsersList);
            return user;
        }

        public bool DeleteUser(int id)
        {
            var user = UsersList.FirstOrDefault(user => user.Id == id);
            if (user == null || user.Id == 0) { return false; }
            if (user!.Id == 1)
            {   
                throw new AdminOpsException("Cannot delete Admin user ID 1");
            }            
            bool valid = UsersList.Remove(user!);
            if (valid)
            {
                JsonIO.WriteJson(JsonPath, UsersList);
                return valid;
            }
            return false;
        }

        public User? FindByEmail(string email)
        {
            return UsersList.FirstOrDefault(user => user.Email == email);
        }

        public User? GetById(int id)
        {
            return UsersList.FirstOrDefault(user => user.Id == id);
        }

        public List<User> ListUsers()
        {
            return UsersList;
        }

        public User UpdateUser(User updatedUser)
        {
            var user = GetById(updatedUser.Id);

            if (user is null)
                throw new Exception("User not found!");

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;
            return user;
        }
    }
}
