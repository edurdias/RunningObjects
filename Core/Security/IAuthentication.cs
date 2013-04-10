using System;
using System.Collections.Generic;
using System.Reflection;

namespace RunningObjects.Core.Security
{
    public interface IAuthentication<out T>
    {
        Type Type { get; }

        Func<IEnumerable<string>> GetRoles();
        bool IsAuthenticated();
        Action LogoutWith();
        MethodInfo LoginWith();

        void GetRolesFrom(Func<IEnumerable<string>> expression);
        void GetAuthenticationStatusFrom(Func<bool> expression);
        void LogoutWith(Action action);
        void LoginWith(Action expression);
        void LoginWith<T1>(Action<T1> expression);
        void LoginWith<T1, T2>(Action<T1, T2> expression);
        void LoginWith<T1, T2, T3>(Action<T1, T2, T3> expression);
        void LoginWith<T1, T2, T3, T4>(Action<T1, T2, T3, T4> expression);
        void LoginWith<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> expression);
        void LoginWith<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> expression);

    }
}