namespace ChossonKallahAdmin.EF6
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContactUsData")]
    public partial class ContactUsData
    {
        public int ContactUsDataID { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(200)]
        public string MessageSubject { get; set; }

        public string DetailedMessage { get; set; }

        public DateTime? AddedDatetime { get; set; }

        [StringLength(20)]
        public string AddedIP { get; set; }
    }
}
