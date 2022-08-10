using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudinskyMartin_Semestralni1CSP
{
    public class MeetingCentre
    {
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public List<MeetingRoom> meetingRooms = new List<MeetingRoom>();
        public string previousCode { get; set; }

        public MeetingCentre(string name, string code, string description)
        {
            this.name = name;
            this.code = code;
            this.description = description;

        }

        /// <summary>
        /// Pretizeni konstruktoru pro pripad zmeny <see cref="code"/> pomoci <param name="previousCode"></param>
        /// </summary>
        public MeetingCentre(string name, string code, string description, string previousCode)
        {
            this.name = name;
            this.code = code;
            this.description = description;
            this.previousCode = previousCode;
        }

        /// <summary>
        /// Urcuje vzhled <see cref="MeetingCentre"/> v <see cref="MainWindow.meetingCentresListBox"/>.
        /// </summary>
        /// <returns>Vraci zformatovanou formu <see cref="name"/> a <see cref="code"/></returns>
        public override string ToString()
        {
            return String.Format("Name: {0}; Code: {1}", name, code);
        }
  
    }
}
