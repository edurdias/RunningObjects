using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RunningObjects.MVC.Security
{
    public class SecurityPolicyContext
    {
        public Func<bool> AuthenticationStatus { get; set; }

        public Func<IEnumerable<string>> CurrentUserRoles { get; set; }

        public ControllerContext ControllerContext { get; set; }
    }
}