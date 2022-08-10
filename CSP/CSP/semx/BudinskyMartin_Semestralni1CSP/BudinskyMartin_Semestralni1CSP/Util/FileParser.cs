using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace BudinskyMartin_Semestralni1CSP.Controllers
{
    //<summary>
    //</summary>
    public static class FileParser
    {
        /// <summary>
        /// Metoda nacte data z csv souboru z dane cesty a vrati <see cref="List{MeetingCentre}"/>
        /// </summary>
        /// <param name="path">cesta k souboru</param>
        public static List<MeetingCentre> importDataFromCSV(string path)
        {
            List<MeetingCentre> meetingCentres = new List<MeetingCentre>();

            //var reader = new StreamReader(@"C:\Users\Zogg\Documents\Visual Studio 2015\Projects\BudinskyMartin_Semestralni1CSP\BudinskyMartin_Semestralni1CSP\Data\ImportData.csv");
            var reader = new StreamReader(path);
            reader.ReadLine(); //skip prvniho headeru
            while (!reader.EndOfStream)
            {
                var meetingCentreData = reader.ReadLine().Split(',');

                if (meetingCentreData.Contains("MEETING_ROOMS"))
                {
                    while (!reader.EndOfStream)
                    {
                        var meetingRoomData = reader.ReadLine().Split(',');
                        if (meetingRoomData.Length <= 1) break;
                        bool hasVideoConference = meetingRoomData[4] == "YES";

                        MeetingRoom m = new MeetingRoom(meetingRoomData[0], meetingRoomData[1], meetingRoomData[2], Convert.ToInt32(meetingRoomData[3]), hasVideoConference);

                        var meetingCentre = meetingCentres.First(c => c.code == meetingRoomData[5]);
                        m.parentMeetingCentre = meetingCentre;
                        meetingCentre.meetingRooms.Add(m);
                    }
                    break;
                }

                meetingCentres.Add(new MeetingCentre(meetingCentreData[0], meetingCentreData[1], meetingCentreData[2]));
            }

            return meetingCentres;
        }

        /// <summary>
        /// Metoda proiteruje Listem a ulozi data do puvodniho csv souboru, ze ktereho bylo importovano. 
        /// </summary>
        /// <param name="meetingCentres"><see cref="List{MeetingCentre}"/> s aktualnimi <see cref="MeetingCentre"/> pro ulozeni.</param>
        /// <param name="path">Cesta k puvodnimu souboru. Pokud neni zadana, je pouzit soubor ExportData.csv ve sloze Data</param>
        public static void exportDataToImportedFile(List<MeetingCentre> meetingCentres, string path)
        {
            string meetingCentresHeader = "MEETING_CENTRES,,,,,";
            string meetingRoomsHeader = "MEETING_ROOMS,,,,,";
            var meetingCentresContent = new StringBuilder();
            var meetingRoomsContent = new StringBuilder();


            if (path == null || path == "")  path = "/Data/ExportData.csv";


            foreach (var meetingCentre in meetingCentres)
            {
                meetingCentresContent.AppendLine(String.Format("{0},{1},{2},,," ,meetingCentre.name, meetingCentre.code,meetingCentre.description));
                foreach (var meetingRoom in meetingCentre.meetingRooms)
                {
                    string hasVideoConference;
                    if (meetingRoom.videoConference) hasVideoConference = "YES";
                    else hasVideoConference = "NO";
                    meetingRoomsContent.AppendLine(String.Format("{0},{1},{2},{3},{4},{5}",meetingRoom.name,meetingRoom.code, meetingRoom.description,meetingRoom.capacity.ToString(),hasVideoConference,meetingRoom.parentMeetingCentre.code));
                }
            }

            var csv = new StringBuilder();
            csv.AppendLine(meetingCentresHeader);
            csv.Append(meetingCentresContent);
            csv.AppendLine(meetingRoomsHeader);
            csv.Append(meetingRoomsContent);
            File.WriteAllText(path, csv.ToString());
        }


    }
}
