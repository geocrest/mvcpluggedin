
namespace Geocrest.Web.Mvc.Admin.Controllers
{
    using Geocrest.Data.Contracts;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Mvc.Controllers;
    using Geocrest.Web.Mvc.Models;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using WebMatrix.WebData;
    /// <summary>
    /// Provides action methods for administering the application's user accounts.
    /// </summary>
    [AdminOnly]
    public class AccountController : BaseController
    {
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Admin.Controllers.AccountController" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public AccountController(IRepository repository)
        {
            Throw.IfArgumentNull(repository, "repository");
            this.repository = repository;
        }
        public virtual ActionResult Index()
        {            
            return View();
        }
        public virtual ActionResult Roles()
        {
            var roles = ((SimpleRoleProvider)System.Web.Security.Roles.Provider).GetAllRoles();
            return View(roles);
        }
        
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public virtual ActionResult DeleteRole(string role)
        {
            var roles = (SimpleRoleProvider)System.Web.Security.Roles.Provider;
            if (roles.RoleExists(role))
            {               
                roles.DeleteRole(role, true);                
            }
            var newroles = ((SimpleRoleProvider)System.Web.Security.Roles.Provider).GetAllRoles();
            return PartialView("roleslist", newroles);
        }
        public virtual ActionResult List()
        {
            ViewBag.Roles = ((SimpleRoleProvider)System.Web.Security.Roles.Provider).GetAllRoles();
            var users = this.repository.All<UserProfile>();
            return PartialView(users);
        }
        public virtual ActionResult CreateRole()
        {
            return PartialView("_createoredit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult CreateRole(string role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var roles = (SimpleRoleProvider)System.Web.Security.Roles.Provider;
                    if (!roles.RoleExists(role))
                    {
                        roles.CreateRole(role);
                    }
                    else
                    {
                        ModelState.AddModelError("duplicate", "The specified role already exists.");
                        return PartialView("_createoredit", role);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(ex.Message, ex);
                    return PartialView("_createoredit", role);
                }
                return Json(new { success = true });
            }
            else
            {
                return PartialView("_createoredit", role);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult AddUserToRole(string username, string role)
        {
            var roles = (SimpleRoleProvider)System.Web.Security.Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;
            if (roles.RoleExists(role))
            {
                if (membership.GetUser(username, false)!= null)
                {
                    if (!roles.GetRolesForUser(username).Contains(role))
                    {
                        roles.AddUsersToRoles(new[] { username }, new[] { role});
                    }
                }
            }
            ViewBag.Roles = ((SimpleRoleProvider)System.Web.Security.Roles.Provider).GetAllRoles();
            var users = this.repository.All<UserProfile>();
            return PartialView("list",users);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult RemoveUserFromRole(string username, string role)
        {
            var roles = (SimpleRoleProvider)System.Web.Security.Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;
            if (roles.RoleExists(role))
            {
                if (membership.GetUser(username, false) != null)
                {
                    if (roles.GetRolesForUser(username).Contains(role))
                    {
                        roles.RemoveUsersFromRoles(new[] { username }, new[] { role });
                    }
                }
            }
            ViewBag.Roles = ((SimpleRoleProvider)System.Web.Security.Roles.Provider).GetAllRoles();
            var users = this.repository.All<UserProfile>();
            return PartialView("list", users);
        }
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public virtual ActionResult DeleteUser(string username)
        {
            // Attempt to register the user
            if(WebSecurity.UserExists(username) && User.Identity.Name != username)
            {
                var membership = (SimpleMembershipProvider)Membership.Provider;
                var roles = System.Web.Security.Roles.Provider.GetRolesForUser(username);
                System.Web.Security.Roles.RemoveUsersFromRoles(new[] { username }, roles);
                membership.DeleteUser(username, true);
            }
            ViewBag.Roles = ((SimpleRoleProvider)System.Web.Security.Roles.Provider).GetAllRoles();
            var users = this.repository.All<UserProfile>();
            return PartialView("list", users);   
        }
    }
}
