using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RunningObjects.MVC.Security
{
    public interface ITypeSecurityConfiguration<out T>
    {

        ISecurityPolicyContainer<T> OnAnything();
        ISecurityPolicyContainer<T> OnWelcome();
        
        ISecurityPolicyContainer<T> OnCreate();
        ISecurityPolicyContainer<T> OnIndex();
        ISecurityPolicyContainer<T> OnView();
        ISecurityPolicyContainer<T> OnEdit();
        ISecurityPolicyContainer<T> OnDelete();

        IEnumerable<ISecurityPolicyContainer<T>> AllExecutions();

        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1>(Func<T, Func<T1, Object>> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn(Func<T, Func<Object>> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2>(Func<T, Func<T1, T2, Object>> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3>(Func<T, Func<T1, T2, T3, Object>> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4>(Func<T, Func<T1, T2, T3, T4, Object>> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5>(Func<T, Func<T1, T2, T3, T4, T5, Object>> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6>(Func<T, Func<T1, T2, T3, T4, T5, T6, Object>> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, Object>> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, Object>> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Object>> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Object>> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Object>> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Object>> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Object>> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Object>> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Object>> expression);

        ISecurityPolicyContainer<T> OnExecute(Func<T, Action> expression);
        ISecurityPolicyContainer<T> OnExecute<T1>(Func<T, Action<T1>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2>(Func<T, Action<T1, T2>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3>(Func<T, Action<T1, T2, T3>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4>(Func<T, Action<T1, T2, T3, T4>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5>(Func<T, Action<T1, T2, T3, T4, T5>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6>(Func<T, Action<T1, T2, T3, T4, T5, T6>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7, T8>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Func<T, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> expression);

        ISecurityPolicyContainer<T> OnExecuteWithReturn(Func<Object> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1>(Func<T1, Object> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2>(Func<T1, T2, Object> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3>(Func<T1, T2, T3, Object> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4>(Func<T1, T2, T3, T4, Object> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, Object> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, Object> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, Object> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, Object> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Object> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Object> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Object> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Object> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Object> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Object> expression);
        ISecurityPolicyContainer<T> OnExecuteWithReturn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Object> expression);

        ISecurityPolicyContainer<T> OnExecute(Action expression);
        ISecurityPolicyContainer<T> OnExecute<T1>(Action<T1> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2>(Action<T1, T2> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3>(Action<T1, T2, T3> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4>(Action<T1, T2, T3, T4> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> expression);



        ISecurityPolicyContainer<T> FindPolicyContainer(ControllerContext controllerContext);
    }
}