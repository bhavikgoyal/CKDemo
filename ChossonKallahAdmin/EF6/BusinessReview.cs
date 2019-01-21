namespace ChossonKallahAdmin.EF6
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BusinessReview")]
    public partial class BusinessReview
    {
        public int BusinessReviewID { get; set; }

        public int? BusinessID { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Email { get; set; }


        public string Review { get; set; }
        

        public double? Rating { get; set; }


        public bool isActive { get; set; }


        public DateTime? AddedOn { get; set; }

        [StringLength(50)]
        public string AddedByIP { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
