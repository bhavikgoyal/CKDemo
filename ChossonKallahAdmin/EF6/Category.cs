namespace ChossonKallahAdmin.EF6
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Category
    {
        public int CategoryId { get; set; }

        [StringLength(100)]
        public string CategoryName { get; set; }

        public int? ParentCategoryID { get; set; }

        [StringLength(100)]
        public string CategoryURL { get; set; }

        public bool IsFeaturedCategory { get; set; }

        public bool IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
    public partial class CategoryClass
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<CategoryClass> childCat { get; set; }
    }
}
