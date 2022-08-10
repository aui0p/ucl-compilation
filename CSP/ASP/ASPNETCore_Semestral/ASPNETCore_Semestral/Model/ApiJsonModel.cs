using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCore_Semestral.Model
{
    //"container" model pro podobu dat Jsonu vracených API
   public class ApiJsonModel
   {
       public string RoomName { get; set; }
       public int OpenFrom { get; set; }
       public int OpenTo { get; set; }
       public string ReservationDate { get; set; }
       public List<string> OpeningTimes { get; set; }
   }
    
}
