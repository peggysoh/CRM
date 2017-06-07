using OgsysCRM.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace OgsysCRM.Infrastructure
{
    public class BaseController : Controller
    {
        protected int UserID
        {
            get
            {
                var id = (FormsIdentity)User.Identity;
                return JsonManager.DeSerialize<UserData>(id.Ticket.UserData).Id;
            }
        }

        protected string FirstName
        {
            get
            {
                var id = (FormsIdentity)User.Identity;
                return JsonManager.DeSerialize<UserData>(id.Ticket.UserData).FirstName;
            }
        }

        protected string LastName
        {
            get
            {
                var id = (FormsIdentity)User.Identity;
                return JsonManager.DeSerialize<UserData>(id.Ticket.UserData).LastName;
            }
        }

        protected string Username
        {
            get { return User.Identity.Name; }
        }

        /// <summary>
        /// Adds TempData with specified Attention message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Attention(string message)
        {
            TempData.Add(Alert.Attention, message);
        }

        /// <summary>
        /// Adds TempData with specified Success message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Success(string message)
        {
            TempData.Add(Alert.Success, message);
        }

        /// <summary>
        /// Adds TempData with specified Information message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Information(string message)
        {
            TempData.Add(Alert.Information, message);
        }

        /// <summary>
        /// Adds TempData with specified Error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(string message)
        {
            TempData.Add(Alert.Error, message);
        }
    } 

    /// <summary>
    /// Alert
    /// </summary>
    public static class Alert
    {
        /// <summary>
        /// The success
        /// </summary>
        public const string Success = "success";

        /// <summary>
        /// The attention
        /// </summary>
        public const string Attention = "attention";

        /// <summary>
        /// The error
        /// </summary>
        public const string Error = "error";

        /// <summary>
        /// The information
        /// </summary>
        public const string Information = "info";

        /// <summary>
        /// Gets the ALL.
        /// </summary>
        public static string[] All
        {
            get { return new[] { Success, Attention, Information, Error }; }
        }
    }
}