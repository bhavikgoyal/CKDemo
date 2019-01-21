namespace ChossonKallahAdmin.EF6
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BusinessGallery")]
    public partial class BusinessGallery
    {
        public int BusinessGalleryID { get; set; }

        public int? BusinessID { get; set; }

        [StringLength(100)]
        public string ImageName { get; set; }

        public bool? IsActive { get; set; }

        public int? Sequence { get; set; }
    }
}
