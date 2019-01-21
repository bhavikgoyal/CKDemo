namespace ChossonKallahAdmin.EF6
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StateData")]
    public partial class StateData
    {
        [Key]
        public int StateId { get; set; }

        [StringLength(500)]
        public string ShortName { get; set; }

        [StringLength(500)]
        public string FullName { get; set; }
    }
}
