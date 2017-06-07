using Newtonsoft.Json;
using OgsysCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OgsysCRM.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AppAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Checks to see if the user is authenticated and has the correct role to access particular view. 
        /// In GNS, user can have only one Role.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>boolean</returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                return false;

            return httpContext.Session != null && httpContext.User.Identity.IsAuthenticated;
        }

        /// <summary>
        /// Processes HTTP requests that fail authorization.
        /// </summary>
        /// <param name="filterContext">Encapsulates the information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />. The <paramref name="filterContext" /> object contains the controller, HTTP context, request context, action result, and route data.</param>
        /// <exception cref="System.ArgumentNullException">filterContext</exception>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            filterContext.Result = new ViewResult { ViewName = "NotAuthorized" };
        }
    } 
}