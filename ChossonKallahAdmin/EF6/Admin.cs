namespace ChossonKallahAdmin.EF6
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Admin")]
    public partial class Admin
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int AdminId { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50)]
        
        public string Name { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(200)]
        public string Username { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(200)]
        public string Email { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(200)]
        public string Password { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool IsActive { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool IsDeleted { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? CreatedOn { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DeletedOn { get; set; }
    }
    

}
