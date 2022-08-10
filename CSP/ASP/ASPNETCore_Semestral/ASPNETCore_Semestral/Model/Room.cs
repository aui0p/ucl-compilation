using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCore_Semestral.Model
{
    [Table("Rooms")]
    public class Room
    {
        public Room()
        {
            Reservations = new List<Reservation>();
        }
        [Required]
        public int RoomId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 50)]
        public string Description { get; set; }

        [Required]
        public int OpenFrom { get; set; }

        [Required]
        public int OpenTo { get; set; }

        public List<Reservation> Reservations = new List<Reservation>();
    }
}
