using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChossonKallahAdmin.EF6
{
    public partial class Listview
    {
        //Admin Property

        public int AdminId { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        //Category Property

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string ParentCategoryName { get; set; }

        public int? ParentCategoryID { get; set; }

        public string CategoryURL { get; set; }

        public bool IsFeaturedCategory { get; set; }

        //Location Property

        public int? LocationId { get; set; }

        public string LocationName { get; set; }

        public string LocationURL { get; set; }

        //Website Banner Property

        public int WebsiteBannerID { get; set; }

        public string BannerName { get; set; }

        public string BannerImage { get; set; }

        public int Sequence { get; set; }

        public string BannerTextLine1 { get; set; }

        public string BannerTextLine2 { get; set; }

        public int? Deletedby { get; set; }

        //Business Review Property

        public int BusinessReviewID { get; set; }

        public int? BusinessID { get; set; }

        public string Review { get; set; }

        public double? Rating { get; set; }

        public string AddedOn { get; set; }

        public string AddedByIP { get; set; }

        //Business Directory Property

        public string BusinessName { get; set; }

        public string Website { get; set; }

        public string BusinessURL { get; set; }

        public string PhoneNumber { get; set; }

        public string BusinessImage { get; set; }

        public string Address { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zipcode { get; set; }

        public string PhoneNumber2 { get; set; }

        public bool HasBrochure { get; set; }

        public bool IsFeatured { get; set; }

        public string BusinessVideoURL { get; set; }

        public string BusinessLogo { get; set; }

        public string Categories { get; set; }

        //Business Gallery
        public int BusinessGalleryID { get; set; }
        
        public string ImageName { get; set; }

        //Vendor

        public int VendorID { get; set; }
        
        public string VendorName { get; set; }
        
        public string VendorEmail { get; set; }
        
        public string VendorPhoneNumber { get; set; }

        public string CreatedAt { get; set; }

        public string DeletedAt { get; set; }

        //ADS Banner

        public int AdsBannerID { get; set; }

        public string AdsBannerName { get; set; }

        public string AdsBannerImage { get; set; }

        public string AdsBannerPosition { get; set; }

        public int? Priority { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string CreateAt { get; set; }

        //General Property
        public bool IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public string CreatedOn { get; set; }

        public string DeletedOn { get; set; }

        public Int32? TotalRecord { get; set; }

    }
}