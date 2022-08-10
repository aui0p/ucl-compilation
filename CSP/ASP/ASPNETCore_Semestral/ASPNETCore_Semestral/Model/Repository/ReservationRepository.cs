using ASPNETCore_Semestral.Model.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCore_Semestral.Model.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        ReservationsContext _repositoryContext;
        public ReservationRepository(ReservationsContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }



        public void CreateReservation(Reservation reservation)
        {
            _repositoryContext.Reservations.Add(reservation);
            _repositoryContext.SaveChanges();
        }

        public Reservation GetById(int id)
        {
            Reservation res = _repositoryContext.Reservations.Single(r => r.RoomId == id);

            return res;
        }

        public List<Reservation> GetAll()
        {
            return _repositoryContext.Reservations.ToList();
        }

        public bool ReservationExists(Reservation reservation)
        {
            return GetAll().Exists(r=> r.ReservedFrom == reservation.ReservedFrom && r.ReservedTo == reservation.ReservedTo);
        }

        public List<Reservation> GetByRoomId(int id)
        {
            return _repositoryContext.Reservations.Where(r => r.RoomId == id).ToList();
        }

        public List<Reservation> GetByRoomIdDate(int id, string date)
        {
            return _repositoryContext.Reservations.Where(r => r.RoomId == id && r.ReservationDate == Convert.ToDateTime(date)).ToList();
        }

    }
}
