using System.ComponentModel.DataAnnotations;
using RunningObjects.MVC.Query;
using System.Web.Script.Serialization;

namespace RunningObjects.MVC.Northwind
{
    [Query(OrderBy = "Region.RegionDescription:Asc,TerritoryDescription:Asc", Paging = true, PageSize = 30)]
    public class Territory
    {
        [Key, Display(Name = "#")]
        public string TerritoryID { get; set; }

        [ScaffoldColumn(false)]
        public int RegionID { get; set; }

        [ScriptIgnore]
        public virtual Region Region { get; set; }

        [Text, Display(Name = "Description")]
        public string TerritoryDescription { get; set; }

        public override string ToString()
        {
            return TerritoryDescription;
        }
    }
}
