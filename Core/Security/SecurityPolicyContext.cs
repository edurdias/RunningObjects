using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RunningObjects.Core.Security
{
    public class SecurityPolicyContext
    {
        private static SecurityPolicyContext Instance;

        public bool IsAuthenticated { get; set; }

        public Func<IEnumerable<string>> CurrentUserRoles { get; set; }

        public ControllerContext ControllerContext { get; set; }

        public static SecurityPolicyContext Current
        {
            get { return Instance ?? (Instance = new SecurityPolicyContext()); }
        }
    }
}