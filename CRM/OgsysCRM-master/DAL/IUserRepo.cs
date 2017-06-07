using OgsysCRM.Models;
using System.Collections.Generic;

namespace OgsysCRM.DAL
{
    public interface IUserRepo
    {
        IEnumerable<User> GetUsers();
        User GetUserById(int userId);
        User GetUserByEmail(string email);
        void InsertUser(User user);
        void DeleteUser(int userId);
        void UpdateUser(User user);
        void Save();
        bool ValidateLogin(string email, string password);
        bool ValidatePassword(int id, string password);
        bool DupblicateEmailCheck(string email);
    }
}
