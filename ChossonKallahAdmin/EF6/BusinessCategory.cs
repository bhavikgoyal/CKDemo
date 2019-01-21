namespace ChossonKallahAdmin.EF6
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BusinessCategory")]
    public partial class BusinessCategory
    {
        public int BusinessCategoryID { get; set; }

        public int? BusinessID { get; set; }

        public int? CategoryID { get; set; }
    }
}
