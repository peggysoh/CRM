using OgsysCRM.Helpers;
using OgsysCRM.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OgsysCRM.DAL
{
    public class UserRepo : IUserRepo
    {
        private AppContext context;

        public UserRepo(AppContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetUsers()
        {
            return context.Users.ToList();
        }

        /// <summary>
        /// Gets the user by Id.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns></returns>
        public User GetUserById(int userId)
        {
            return context.Users.Find(userId);
        }

        /// <summary>
        /// Gets the user by email.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <returns></returns>
        public User GetUserByEmail(string email)
        {
            return context.Users.SingleOrDefault(x => x.Email == email);
        }

        /// <summary>
        /// Inserts the user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void InsertUser(User user)
        {
            var salt = "";
            user.Email = user.Email.ToLower();
            user.Password = SecurityHelper.HashPassword(user.Password, ref salt);
            user.Salt = salt;

            context.Users.Add(user);
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        public void DeleteUser(int userId)
        {
            User user = context.Users.Find(userId);
            context.Users.Remove(user);
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void UpdateUser(User user)
        {
            context.Entry(user).State = EntityState.Modified;
        }

        /// <summary>
        /// Saves context.
        /// </summary>
        public void Save()
        {
            context.SaveChanges();
        }

        /// <summary>
        /// Validates login.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool ValidateLogin(string email, string password)
        {
            var user = context.Users.SingleOrDefault(x => x.Email == email);
            if (user == null)
                return false;

            var salt = user.Salt;
            var hashedPassword = SecurityHelper.HashPassword(password: password, salt: ref salt);
            return user.Password == hashedPassword;
        }

        /// <summary>
        /// Validates password only.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool ValidatePassword(int id, string password)
        {
            var user = context.Users.SingleOrDefault(x => x.Id == id);
            if (user == null)
                return false;

            var salt = user.Salt;
            var hashedPassword = SecurityHelper.HashPassword(password: password, salt: ref salt);
            return user.Password == hashedPassword;
        }

        /// <summary>
        /// Checks if email exists.
        /// </summary>
        /// <param name="user">The email.</param>
        /// <returns></returns>
        public bool DupblicateEmailCheck(string email)
        {
            var user = context.Users.SingleOrDefault(x => x.Email == email);
            return user != null;
        }
        
    }
}