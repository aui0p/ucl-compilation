using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudinskyMartin_Semestralni1CSP
{
    public class MeetingRoom
    {
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public int capacity { get; set; }
        public bool videoConference { get; set; }
        public MeetingCentre parentMeetingCentre { get; set; }
        public string previousCode { get; set; }

        public MeetingRoom(string name, string code, string description, int capacity, bool videoConference)
        {
            this.name = name;
            this.code = code;
            this.description = description;
            this.capacity = capacity;
            this.videoConference = videoConference;
        }

        /// <summary>
        /// Pretizeni konstruktoru pro pripad zmeny <see cref="code"/> pomoci <param name="previousCode"></param>
        /// </summary>
        public MeetingRoom(string name, string code, string description, int capacity, bool videoConference, string previousCode)
        {
            this.name = name;
            this.code = code;
            this.description = description;
            this.capacity = capacity;
            this.videoConference = videoConference;
            this.previousCode = previousCode;
        }

        /// <summary>
        /// Urcuje vzhled <see cref="MeetingRoom"/> v <see cref="MainWindow.meetingRoomsListBox"/>.
        /// </summary>
        /// <returns>Vraci zformatovanou formu <see cref="name"/> a <see cref="code"/></returns>
        public override string ToString()
        {
            return String.Format("Name: {0}; Code: {1}", name, code);
        }
        
    }
}
