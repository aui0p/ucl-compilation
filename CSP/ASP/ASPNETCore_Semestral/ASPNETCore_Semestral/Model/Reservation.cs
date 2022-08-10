using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCore_Semestral.Model
{
    [Table("Reservations")]
    public class Reservation
    {
        [Required]
        public int ReservationId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ReservationDate { get; set; }

        [Required]
        public int ReservedFrom { get; set; }

        [Required]
        public int ReservedTo { get; set; }

        [Required]
        [StringLength(50)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(50)]
        public string CustomerSurname { get; set; }

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; }

        [Required]
        [Phone]
        public string CustomerPhone { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

    }

}
