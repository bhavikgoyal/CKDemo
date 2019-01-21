namespace ChossonKallahAdmin.EF6
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Vendor")]
    public partial class Vendor
    {
        public int VendorID { get; set; }

        [StringLength(100)]
        public string VendorName { get; set; }

        [StringLength(100)]
        public string VendorEmail { get; set; }

        [StringLength(50)]
        public string VendorPhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
