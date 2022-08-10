using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCore_Semestral.Model.Repository
{
    public interface IRoomRepository
    {
        //room db methods
        List<Room> GetAll();
        Room GetById(int id);
    }
}
