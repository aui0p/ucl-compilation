using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudinskyMartin_Semestralni1CSP.Controllers
{
    public struct ModalActionController
    {
        /// <summary>
        /// Seznam proveditelnych akci modalnim oknem.
        /// </summary>
        public enum ActionType
        {
            AddNewMeetingCentre,
            UpdateMeetingCentre,
            AddNewMeetingRoom,
            UpdateMeetingRoom,
            RemoveMeetingCentre,
            RemoveMeetingRoom
        }

        /// <summary>
        /// Seznam moznych typu obsahu modalniho okna.
        /// </summary>
        public enum ContentType
        {
            MeetingCentre,
            MeetingRoom
        }
    }
}
