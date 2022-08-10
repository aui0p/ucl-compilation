namespace EntityGarbageWPF
{
    using Entity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Accessory
    {
        public int AccessoryId { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public int Amount { get; set; }

        public int MinAmount { get; set; }

        public bool ToBeShown { get; set; }

        public virtual Category Category { get; set; }
        
        
        public override string ToString()
        {
            return this.Name;
        }
    }
}
