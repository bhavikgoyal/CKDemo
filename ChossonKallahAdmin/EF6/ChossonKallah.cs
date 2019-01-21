namespace ChossonKallahAdmin.EF6
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ChossonKallah : DbContext
    {
        public ChossonKallah()
            : base("name=ChossonKallah")
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<AdsBanner> AdsBanners { get; set; }
        public virtual DbSet<BusinessCategory> BusinessCategories { get; set; }
        public virtual DbSet<BusinessDirectory> BusinessDirectories { get; set; }
        public virtual DbSet<BusinessGallery> BusinessGalleries { get; set; }
        public virtual DbSet<BusinessReview> BusinessReviews { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ContactUsData> ContactUsDatas { get; set; }
        public virtual DbSet<GeneralSetting> GeneralSettings { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<StateData> StateDatas { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<WebsiteBanner> WebsiteBanners { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<AdsBanner>()
                .Property(e => e.AdsBannerName)
                .IsUnicode(false);

            modelBuilder.Entity<AdsBanner>()
                .Property(e => e.AdsBannerImage)
                .IsUnicode(false);

            modelBuilder.Entity<AdsBanner>()
                .Property(e => e.AdsBannerPosition)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessDirectory>()
                .Property(e => e.BusinessName)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessDirectory>()
                .Property(e => e.Website)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessDirectory>()
                .Property(e => e.BusinessURL)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessDirectory>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessDirectory>()
                .Property(e => e.BusinessImage)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessDirectory>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessDirectory>()
                .Property(e => e.AddressLine2)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessDirectory>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessDirectory>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessDirectory>()
                .Property(e => e.Zipcode)
                .IsUnicode(false);

           

            modelBuilder.Entity<BusinessDirectory>()
                .Property(e => e.PhoneNumber2)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessDirectory>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessDirectory>()
                .Property(e => e.BusinessVideoURL)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessDirectory>()
                .Property(e => e.BusinessLogo)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessGallery>()
                .Property(e => e.ImageName)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessReview>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessReview>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessReview>()
                .Property(e => e.Review)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessReview>()
                .Property(e => e.AddedByIP)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.CategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.CategoryURL)
                .IsUnicode(false);

            modelBuilder.Entity<ContactUsData>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ContactUsData>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<ContactUsData>()
                .Property(e => e.MessageSubject)
                .IsUnicode(false);

            modelBuilder.Entity<GeneralSetting>()
                .Property(e => e.Key)
                .IsUnicode(false);

            modelBuilder.Entity<GeneralSetting>()
                .Property(e => e.Descrpition)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.LocationName)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.LocationURL)
                .IsUnicode(false);

            modelBuilder.Entity<StateData>()
                .Property(e => e.ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<StateData>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.VendorName)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.VendorEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Vendor>()
                .Property(e => e.VendorPhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<WebsiteBanner>()
                .Property(e => e.BannerName)
                .IsUnicode(false);

            modelBuilder.Entity<WebsiteBanner>()
                .Property(e => e.BannerImage)
                .IsUnicode(false);
        }
    }
}
