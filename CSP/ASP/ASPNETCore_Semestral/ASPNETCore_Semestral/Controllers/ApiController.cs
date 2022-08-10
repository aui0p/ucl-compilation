using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNETCore_Semestral.Model;
using ASPNETCore_Semestral.Model.Repository;

namespace ASPNETCore_Semestral.Controllers
{
    [Route("api/[controller]")]
    public class RoomController : Controller
    {
        IRoomRepository _roomRepository; IReservationRepository _reservationRepository;
        public RoomController(IRoomRepository roomRepository, IReservationRepository reservationRepository)
        {
            _roomRepository = roomRepository;
            _reservationRepository = reservationRepository;
        }
        // GET api/room/15.05.2017
        [HttpGet("{date}")]
        [Route("api/room/{date}")]
        public IActionResult GetByDate(string date)
        {
            List<ApiJsonModel> jsonData = new List<ApiJsonModel>();
            List<Room> Rooms = _roomRepository.GetAll();

            foreach (var room in Rooms)
            {
                room.Reservations = _reservationRepository.GetByRoomIdDate(room.RoomId, date);
                List<string> openingDates = findOpeningTimes(room);

                jsonData.Add(new ApiJsonModel { RoomName = room.Name, OpenFrom = room.OpenFrom, OpenTo = room.OpenTo, OpeningTimes = openingDates, ReservationDate = date });
            }
            return Json(jsonData);
        }

        // GET api/room/15.05.2017/2
        [HttpGet("{date}/{roomID}")]
        [Route("api/room/{date}/{roomID}")]
        public IActionResult GetByDateId(string date, int roomID)
        {
            List<ApiJsonModel> jsonData = new List<ApiJsonModel>();
            Room Room = _roomRepository.GetById(roomID);
                
            Room.Reservations = _reservationRepository.GetByRoomIdDate(Room.RoomId, date);
            List<string> openingDates = findOpeningTimes(Room);
            jsonData.Add(new ApiJsonModel { RoomName = Room.Name, OpenFrom = Room.OpenFrom, OpenTo = Room.OpenTo, OpeningTimes = openingDates, ReservationDate = date });

            
            return Json(jsonData);
        }

        
        private List<string> findOpeningTimes(Room room)
        {
            List<string> openingDates = new List<string>();
            for (int i = room.OpenFrom; i < room.OpenTo; i++)
            {
                if (room.Reservations.Exists(x => x.ReservedFrom == i && x.ReservedTo == i + 1))
                    continue;
                else
                    openingDates.Add($"{i}:00-{i + 1}:00");

            }
            return openingDates;
        }
    }
}
