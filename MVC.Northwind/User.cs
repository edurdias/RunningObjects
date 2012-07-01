using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace RunningObjects.MVC.Northwind
{
    public class User
    {
        public IEnumerable<string> Roles { get; set; }

        public static void Login
        (
            [Required, Display(Name = "Username")] string username,
            [Required, Display(Name = "Password")] string password,
            [Display(Name = "Remember Me")] bool rememberMe
        )
        {
            HttpContext.Current.Session["user"] = new User { Roles = new[] { "Admin" } };
        }

        public static void Register([Required] string name)
        {
            
        }

        public static void Logout()
        {
            HttpContext.Current.Session.Remove("user");
        }
    }
}