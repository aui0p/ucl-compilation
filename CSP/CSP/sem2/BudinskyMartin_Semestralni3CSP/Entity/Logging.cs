namespace EntityGarbageWPF.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Logging
    {
        public int LoggingId { get; set; }

        public int AccessoryId { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Customer { get; set; }

        public int Amount { get; set; }

        public DateTime Date { get; set; }
        
    }
}
