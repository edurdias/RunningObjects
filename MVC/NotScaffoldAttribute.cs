using System.ComponentModel.DataAnnotations;

namespace RunningObjects.MVC
{
    public class NotScaffoldAttribute : ScaffoldColumnAttribute
    {
        public NotScaffoldAttribute()
            : base(false)
        {
        }
    }
}
