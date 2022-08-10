using ASPNETCore_Semestral.Model.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCore_Semestral.Model.Repository
{
    public class RoomRepository : IRoomRepository
    {
        ReservationsContext _repositoryContext;

        public RoomRepository(ReservationsContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            //prvotní vložení dat do db
            if (!repositoryContext.Rooms.Any())
            {
                _repositoryContext.Rooms.Add(new Room() { Name = "Room 1", OpenFrom = 10, OpenTo = 20, Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In tempor et dolor sit amet accumsan. Phasellus a mattis sapien. Etiam neque justo, porttitor eu scelerisque id, accumsan vitae lorem. Nulla facilisi. Fusce a elit sit amet eros lacinia cursus id tempor sapien. Integer vitae quam sed mauris congue accumsan at malesuada ex. In eget mi egestas, lacinia purus eu, consectetur orci. Vivamus faucibus porta lorem, vel fermentum libero. Donec congue aliquet ultricies. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas." });
                _repositoryContext.Rooms.Add(new Room() { Name = "Room 2", OpenFrom = 8, OpenTo = 18, Description = "Pellentesque et velit nunc. In lacus metus, euismod vitae leo in, condimentum malesuada tortor. Sed lobortis pretium metus eget volutpat. Donec mollis venenatis magna vestibulum egestas. Etiam eleifend, ante in posuere egestas, tellus lorem consectetur quam, eget porttitor ante lacus vel erat. Aliquam ac leo urna. Proin at sem sed nisl vestibulum rutrum. Nullam pulvinar mi in justo rhoncus luctus. Vivamus lacinia volutpat lectus et varius." });
                _repositoryContext.Rooms.Add(new Room() { Name = "Room 3", OpenFrom = 9, OpenTo = 21, Description = "Nulla facilisi. Proin a ante a orci ullamcorper facilisis. Cras sed massa lacus. Fusce pharetra mauris id odio ultrices, interdum venenatis massa pellentesque. Vestibulum facilisis auctor orci id fringilla. Suspendisse potenti. Donec urna metus, eleifend a suscipit quis, pellentesque quis nibh." });
                _repositoryContext.SaveChanges();
            }
        }

        public List<Room> GetAll()
        {
            return _repositoryContext.Rooms.ToList();
        }

        public Room GetById(int id)
        {
            return _repositoryContext.Rooms.Single(r => r.RoomId == id);
        }
    }
}
