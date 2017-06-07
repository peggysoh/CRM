using System.Web;
using System.Web.Mvc;
using OgsysCRM.Models;
using OgsysCRM.Infrastructure;
using OgsysCRM.DAL;
using System.Web.Security;
using System;
using OgsysCRM.Helpers;

namespace OgsysCRM.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserRepo _userRepo;

        public AccountController()
        {
            _userRepo = new UserRepo(new AppContext());
        }

        [AppAuthorize]
        public ActionResult Index()
        {
            try
            {
                var user = _userRepo.GetUserById(UserID);
                var model = new AccountViewModel(user);
                model.EditError = 0;

                return View(model);
            }
            catch (Exception)
            {
                return View();
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            var model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (_userRepo.ValidateLogin(model.Email, model.Password))
            {
                var now = DateTime.UtcNow.ToLocalTime();
                var user = _userRepo.GetUserByEmail(model.Email);
                var userData = new UserData
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };

                var formsAuthticket = new FormsAuthenticationTicket(
                            version: 1,
                            name: user.FirstName + " " + user.LastName,
                            issueDate: now,
                            expiration: now.AddDays(7),
                            isPersistent: model.RememberMe,
                            userData: JsonManager.Serialize(userData),
                            cookiePath: FormsAuthentication.FormsCookiePath);

                var encryptedTicket = FormsAuthentication.Encrypt(ticket: formsAuthticket);
                var httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                httpCookie.HttpOnly = true;
                if (formsAuthticket.IsPersistent) httpCookie.Expires = formsAuthticket.Expiration;

                Response.Cookies.Set(httpCookie);

                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Customer");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
        }
        
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_userRepo.DupblicateEmailCheck(model.Email))
                {
                    ModelState.AddModelError("", "This user already exists..");
                    return View(model);
                }

                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password
                };

                _userRepo.InsertUser(user);
                _userRepo.Save();
                return RedirectToAction("Login", "Account");
            }
            
            return View(model);
        }
        
        public ActionResult Logout()
        {
            if ((System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AppAuthorize]
        public ActionResult Index(AccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_userRepo.ValidatePassword(UserID, model.OldPassword))
            {
                var logoutFlag = false;

                var user = _userRepo.GetUserById(UserID);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                if (user.Email != model.Email.ToLower())
                {
                    var duplicate = _userRepo.DupblicateEmailCheck(model.Email);
                    if (duplicate)
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("", "Email is in use.");
                        model.EditError = 1;
                        return View(model);
                    }
                    user.Email = model.Email.ToLower();
                    logoutFlag = true;
                }
                
                if (!String.IsNullOrWhiteSpace(model.Password))
                {
                    if (model.Password.Length < 6)
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("", "The password must be at least 6 characters long.");
                        model.EditError = 1;
                        return View(model);
                    }

                    var salt = "";
                    user.Password = SecurityHelper.HashPassword(model.Password, ref salt);
                    user.Salt = salt;
                    logoutFlag = true;
                }
                _userRepo.UpdateUser(user);
                _userRepo.Save();

                if (logoutFlag)
                {
                    TempData["Success"] = "Email/Password changed. Please log back in.";
                    return RedirectToAction("Logout", "Account");
                }
                else
                {
                    TempData["Success"] = "Account updated.";
                    return RedirectToAction("Index", "Account");
                }                    
            }
            else
            {
                ModelState.Clear();
                ModelState.AddModelError("", "The old password is not valid.");
                model.EditError = 1;
                return View(model);
            }
        }
    }
}