namespace ChossonKallahAdmin.EF6
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WebsiteBanner")]
    public partial class WebsiteBanner
    {
        public int WebsiteBannerID { get; set; }

        [StringLength(100)]
        public string BannerName { get; set; }

        [StringLength(500)]
        public string BannerImage { get; set; }

        public bool IsActive { get; set; }

        public int Sequence { get; set; }

        [StringLength(50)]
        public string BannerTextLine1 { get; set; }

        [StringLength(50)]
        public string BannerTextLine2 { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? Deletedby { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
