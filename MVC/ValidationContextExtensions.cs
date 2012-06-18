using System.ComponentModel.DataAnnotations;
using System.Data;

namespace RunningObjects.MVC
{
    public static class ValidationContextExtensions
    {
        public static EntityState State(this ValidationContext context)
        {
            return (EntityState) context.Items["state"];
        }

        public static void State(this ValidationContext context, EntityState state)
        {
            context.Items["state"] = state;
        }
    }
}