using System;

namespace ChossonKallahAdmin.EF6
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BusinessDirectory")]
    public partial class BusinessDirectory
    {
        [Key]
        public int BusinessID { get; set; }

        [StringLength(200)]
        public string BusinessName { get; set; }

        [StringLength(100)]
        public string Website { get; set; }

        [StringLength(100)]
        public string BusinessURL { get; set; }

        [StringLength(200)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string BusinessImage { get; set; }

        public int? LocationId { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(500)]
        public string AddressLine2 { get; set; }

        [StringLength(500)]
        public string City { get; set; }

        [StringLength(500)]
        public string State { get; set; }

        [StringLength(5)]
        public string Zipcode { get; set; }
        
        public bool IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        [StringLength(200)]
        public string PhoneNumber2 { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        public bool HasBrochure { get; set; }

        public bool IsFeatured { get; set; }

        [StringLength(1000)]
        public string BusinessVideoURL { get; set; }

        [StringLength(100)]
        public string BusinessLogo { get; set; }

    }

    public partial class BusinessDirectoryClass
    {

        public int BusinessID { get; set; }

        public string BusinessName { get; set; }

        public string Website { get; set; }

        public string BusinessURL { get; set; }

        public string PhoneNumber { get; set; }

        public string BusinessImage { get; set; }

        public int? LocationId { get; set; }

        public string Address { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zipcode { get; set; }

        public bool IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string PhoneNumber2 { get; set; }

        public string Email { get; set; }

        public bool HasBrochure { get; set; }

        public bool IsFeatured { get; set; }

        public string BusinessVideoURL { get; set; }

        public string BusinessLogo { get; set; }

        public List<BusinessGalleryClass> Gallery { get; set; }

    }
    public partial class BusinessGalleryClass
    {
        public int BusinessGalleryID { get; set; }

        public int? BusinessID { get; set; }

        public string ImageName { get; set; }

        public bool? IsActive { get; set; }

        public int? Sequence { get; set; }
    }
}
