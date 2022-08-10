using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNETCore_Semestral.Model.Repository;
using ASPNETCore_Semestral.Model;
using ASPNETCore_Semestral.ViewModels;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASPNETCore_Semestral.Controllers
{
    public class RoomsController : Controller
    {
        private IRoomRepository _roomRepository;
        private IReservationRepository _reservationRepository;
        public RoomsController(IRoomRepository roomRepository, IReservationRepository reservationRepository)
        {
            _roomRepository = roomRepository;
            _reservationRepository = reservationRepository;
        }


        // GET: /Rooms/
        public IActionResult Index()
        {
            return View(_roomRepository.GetAll());
        }
        
        public IActionResult RoomDetail(int id)
        {
            Room room = _roomRepository.GetAll().Single(r => r.RoomId == id);
            room.Reservations = _reservationRepository.GetByRoomId(id);
            return View(room);
        }

       

        //metoda rozparsuje příchozí date, najde podle id Room a jeho příslušné rezervace podle date
        //zjistí volné rezervační časy a vyrenderuje partial view s nimi
        public IActionResult RoomOpeningsAsync(int id, string date)
        {
            int roomId = Convert.ToInt32(id);
            Room room = _roomRepository.GetAll().Single(r => r.RoomId == roomId);
            string[] dateToSplit = date.Split('/');
            string dateToConvert = $"{dateToSplit[1]}.{dateToSplit[0]}.{dateToSplit[2]}";
            room.Reservations = _reservationRepository.GetByRoomIdDate(roomId,dateToConvert);
            List<string> openingDates = new List<string>();

            for (int i = room.OpenFrom; i < room.OpenTo; i++)
            {
                if (room.Reservations.Exists(x => x.ReservedFrom == i && x.ReservedTo == i + 1))
                    continue;
                else
                    openingDates.Add($"{i}:00-{i + 1}:00");

            }
            ReservationTimesViewModel vm = new ReservationTimesViewModel();
            vm.ReservationTimes = openingDates;
            return PartialView("OpeningTimes", vm);
        }

        //metoda přijme data z formuláře (hlavně kvůli hidden inputům)
        //rozparsuje vybraný čas, podle id najde používaný room a vytvoří novou instanci rezervace, 
        //tu předá do ProcessReservation View, kde jsou do ní přidána další data
        [HttpPost]
        public IActionResult ProcessReservation(IFormCollection collection)
        {
            int roomId = Convert.ToInt32(collection["RoomId"]);
            string selectedTime = collection["SelectedTime"];
            string selectedDate = collection["SelectedDate"];

            string[] timeToSplit = selectedTime.Split(':', '-');
            int hoursFrom = Convert.ToInt32(timeToSplit[0]);
            int hoursTo = Convert.ToInt32(timeToSplit[2]);

            string[] dateToSplit = selectedDate.Split('/');
            string dateToConvert = $"{dateToSplit[1]}.{dateToSplit[0]}.{dateToSplit[2]}";

            Room room = _roomRepository.GetAll().Single(r => r.RoomId == roomId);

            Reservation res = new Reservation() { Room = room, RoomId = room.RoomId,ReservationDate = Convert.ToDateTime(dateToConvert), ReservedFrom = hoursFrom, ReservedTo = hoursTo };
            return View(res);
        }

        //binduje rezervaci doplněnou o data z formuláře, zkontroluje validitu a na základě
        //úspěchu/neúspěchu přesměruje uživatele na potřebnou stránku i s vypsáním chybové zprávy
        [HttpPost]
        public IActionResult CreateReservation(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                if (_reservationRepository.ReservationExists(reservation))
                {
                    TempData["Message"] = "Reservation at this time already exists! Please choose other";
                    return RedirectToAction("RoomDetail/" + reservation.RoomId);
                }
                else
                {
                    _reservationRepository.CreateReservation(reservation);
                    TempData["Message"] = "Reservation has been successfuly created!";
                    return RedirectToAction("Index");
                }
                
            }
            TempData["Message"] = "Data is not valid!";
            return View("ProcessReservation", reservation);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
