using System.Dynamic;

namespace RunningObjects.MVC
{
    public class MemberBinder : SetMemberBinder
    {
        public MemberBinder(string name)
            : base(name, false)
        {
        }

        public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
        {
            return value;
        }
    }
}