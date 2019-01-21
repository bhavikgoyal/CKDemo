namespace ChossonKallahAdmin.EF6
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AdsBanner")]
    public partial class AdsBanner
    {
        public int AdsBannerID { get; set; }

        [StringLength(100)]
        public string AdsBannerName { get; set; }

        [StringLength(100)]
        public string AdsBannerImage { get; set; }

        [StringLength(50)]
        public string AdsBannerPosition { get; set; }

        public int? Priority { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public int? VendorID { get; set; }

        public int? CategoryID { get; set; }

        public bool IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreateAt { get; set; }
    }
}
