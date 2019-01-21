namespace ChossonKallahAdmin.EF6
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GeneralSetting
    {
        [Key]
        public int GeneralSettings { get; set; }

        [StringLength(100)]
        public string Key { get; set; }

        [StringLength(100)]
        public string Descrpition { get; set; }
    }
}
