using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudinskyMartin_Semestralni1CSP.Util;


namespace BudinskyMartin_Semestralni1CSP.Controllers
{
    public class MeetingController
    {

        private List<MeetingCentre> defaultCentres = new List<MeetingCentre>();
        private string pathToFile;

        /// <summary>
        /// Prida <param name="meetingCentre"></param> do <see cref="defaultCentres"/>.
        /// </summary>
        public void addMeetingCentre(MeetingCentre meetingCentre)
        {
            defaultCentres.Add(meetingCentre);
        }



        /// <summary>
        /// Proiteruje <see cref="defaultCentres"/> a najde <see cref="MeetingCentre"/> podle <param name="meetingCentre"><see cref="MeetingCentre.previousCode"/></param>.
        /// Pokud takove existuje, upravi jeho vlastnosti podle noveho.
        /// </summary>
        public void updateMeetingCentre(MeetingCentre meetingCentre)
        {
            var c = defaultCentres.First(mc => mc.code == meetingCentre.previousCode);
            if (c != null) { c.name = meetingCentre.name; c.code = meetingCentre.code; c.description = meetingCentre.description;  }
        }



        /// <summary>
        /// Prida <param name="meetingRoom"></param> do <param name="parentMeetingCentre"><see cref="MeetingCentre.meetingRooms"/>.
        /// </summary>
        public void addMeetingRoom(MeetingRoom meetingRoom, MeetingCentre parentMeetingCentre)
        {
            if (parentMeetingCentre != null) parentMeetingCentre.meetingRooms.Add(meetingRoom);
        }



        /// <summary>
        /// Proiteruje <param name="parentMeetingCentre"><see cref="MeetingCentre.meetingRooms"/></param> a najde <param name="meetingRoom"></param> podle jeho <see cref="MeetingRoom.previousCode"/>.
        /// Pokud takovy existuje, upravi jeho vlastnosti podle noveho.
        /// </summary>
        public void updateMeetingRoom(MeetingRoom meetingRoom, MeetingCentre parentMeetingCentre)
        {
            if (parentMeetingCentre != null)
            {
                var r = parentMeetingCentre.meetingRooms.First(mr => mr.code == meetingRoom.previousCode);
                if (r != null) { r.code = meetingRoom.code; r.name = meetingRoom.name; r.description = meetingRoom.description; r.capacity = meetingRoom.capacity; r.videoConference = meetingRoom.videoConference; r.parentMeetingCentre = meetingRoom.parentMeetingCentre; }
            }
        }



        /// <summary>
        /// Odstrani <param name="meetingCentre"></param> z <see cref="defaultCentres"/>.
        /// </summary>
        public void removeMeetingCentre(MeetingCentre meetingCentre)
        {
            defaultCentres.Remove(meetingCentre);
        }



        /// <summary>
        /// Odstrani <param name="meetingRoom"></param> z  <param name="parentMeetingCentre"><see cref="MeetingCentre.meetingRooms"/>.
        /// </summary>
        public void removeMeetingRoom(MeetingRoom meetingRoom, MeetingCentre parentMeetingCentre)
        {
            if (parentMeetingCentre != null) parentMeetingCentre.meetingRooms.Remove(meetingRoom);
        }



        /// <summary>
        /// Vrati <see cref="List{MeetingCentre}"/>
        /// </summary>
        public List<MeetingCentre> getMeetingCentres()
        {
            return this.defaultCentres;
        }



        /// <summary>
        /// Zavola <see cref="FileParser.importDataFromCSV(string)"/> a data ulozi do <see cref="defaultCentres"/>.
        /// <param name="path"></param> ulozi do <see cref="pathToFile"/>.
        /// </summary>
        public void loadMeetingCentresFromFile(string path)
        {
            defaultCentres = FileParser.importDataFromCSV(path);
            pathToFile = path;
        }



        /// <summary>
        /// Vrati <see cref="pathToFile"/>
        /// </summary>
        public string getPathToImportedFile()
        {
            return pathToFile;
        }
    }
}
