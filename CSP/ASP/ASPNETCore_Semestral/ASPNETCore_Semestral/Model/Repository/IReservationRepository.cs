using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCore_Semestral.Model.Repository
{
    public interface IReservationRepository
    {
        //reservations db metody
        Reservation GetById(int id);
        List<Reservation> GetByRoomId(int id);
        List<Reservation> GetByRoomIdDate(int id, string date);
        void CreateReservation(Reservation reservation);
        List<Reservation> GetAll();
        bool ReservationExists(Reservation reservation);
    }
}
