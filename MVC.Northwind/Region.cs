using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace RunningObjects.MVC.Northwind
{
    public class Region
    {
        public Region()
        {
        }

        public Region
        (
            [Required, Display(Name = "Description")] string description
        )
        {
            if(string.IsNullOrEmpty(description))
                throw new ArgumentNullException("description");
            RegionDescription = description;
        }

        [Key, Display(Name = "#")]
        public int RegionID { get; set; }

        [Text, Display(Name = "Description")]
        public string RegionDescription { get; set; }

        [ScriptIgnore]
        [Query(Select = "TerritoryID,TerritoryDescription", OrderBy = "TerritoryDescription:Asc")]
        public virtual ICollection<Territory> Territories { get; set; }

        public override string ToString()
        {
            return RegionDescription;
        }
    }
}
