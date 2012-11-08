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

        ISecurityPolicyContainer<T> OnExecute<TResult>(Func<T, Func<TResult>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, TResult>(Func<T, Func<T1, TResult>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, TResult>(Func<T, Func<T1, T2, TResult>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, TResult>(Func<T, Func<T1, T2, T3, TResult>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, TResult>(Func<T, Func<T1, T2, T3, T4, TResult>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, TResult>(Func<T, Func<T1, T2, T3, T4, T5, TResult>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, TResult>(Func<T, Func<T1, T2, T3, T4, T5, T6, TResult>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, TResult>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> expression);

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

        ISecurityPolicyContainer<T> OnExecute<TResult>(Func<TResult> expression);
        ISecurityPolicyContainer<T> OnExecute<T1,TResult>(Func<T1, TResult> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2,TResult>(Func<T1, T2, TResult> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3,TResult>(Func<T1, T2, T3, TResult> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4,TResult>(Func<T1, T2, T3, T4, TResult> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5,TResult>(Func<T1, T2, T3, T4, T5, TResult> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6,TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7,TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8,TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9,TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> expression);
        ISecurityPolicyContainer<T> OnExecute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> expression);

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