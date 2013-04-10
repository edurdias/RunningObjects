using System;
using System.Collections.Generic;
using System.Reflection;

namespace RunningObjects.Core.Security
{
    public class Authentication<T> : IAuthentication<T> where T : class
    {
        public Authentication(SecurityConfigurationBuilder builder)
        {
            Builder = builder;
        }

        public SecurityConfigurationBuilder Builder { get; set; }
        public Type Type { get { return typeof(T); } }

        private Func<IEnumerable<string>> getRolesFrom;
        private Func<bool> getAuthenticationStatus;
        private Action logoutWith;
        private MethodInfo loginWith;

        private void LoginWith(MethodInfo method)
        {
            if (method.DeclaringType != typeof(T))
                throw new ArgumentException("Invalid declaring type for the LoginWith expression.", "method");
            loginWith = method;

            var configuration = Builder.For<T>() as TypeSecurityConfiguration<T>;
            if (configuration != null)
                configuration.OnExecute(method).AllowAll();
        }

        #region Implementation of IAuthentication<out T>

        public Func<IEnumerable<string>> GetRoles()
        {
            return getRolesFrom;
        }

        public bool IsAuthenticated()
        {
            return getAuthenticationStatus != null && getAuthenticationStatus();
        }

        public Action LogoutWith()
        {
            return logoutWith;
        }

        public MethodInfo LoginWith()
        {
            return loginWith;
        }

        public void GetRolesFrom(Func<IEnumerable<string>> expression)
        {
            getRolesFrom = expression;
        }

        public void GetAuthenticationStatusFrom(Func<bool> expression)
        {
            getAuthenticationStatus = expression;
        }

        public void LogoutWith(Action action)
        {
            logoutWith = action;
        }

        public void LoginWith(Action expression)
        {
            LoginWith(expression.Method);
        }

        public void LoginWith<T1>(Action<T1> expression)
        {
            LoginWith(expression.Method);
        }

        public void LoginWith<T1, T2>(Action<T1, T2> expression)
        {
            LoginWith(expression.Method);
        }

        public void LoginWith<T1, T2, T3>(Action<T1, T2, T3> expression)
        {
            LoginWith(expression.Method);
        }

        public void LoginWith<T1, T2, T3, T4>(Action<T1, T2, T3, T4> expression)
        {
            LoginWith(expression.Method);
        }

        public void LoginWith<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> expression)
        {
            LoginWith(expression.Method);
        }

        public void LoginWith<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> expression)
        {
            LoginWith(expression.Method);
        }

        #endregion


    }
}