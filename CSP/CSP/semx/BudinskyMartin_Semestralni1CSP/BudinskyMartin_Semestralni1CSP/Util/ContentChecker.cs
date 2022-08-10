using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BudinskyMartin_Semestralni1CSP.Util
{
    public static class ContentChecker
    {
        /// <summary>
        /// Kontroluje, jestli uzivatel vybral <see cref="MeetingCentre"/> z <see cref="MainWindow.meetingCentresListBox"/> pred vykonanim akce.
        /// </summary>
        public static bool isSelectedMeetingCentre(MeetingCentre meetingCentre)
        {
            if (meetingCentre == null) { MessageBox.Show("No Meeting Centre selected!"); return false; }
            else return true;
        }

        /// <summary>
        /// Kontroluje, jestli uzivatel vybral <see cref="MeetingRoom"/> z <see cref="MainWindow.meetingRoomsListBox"/> pred vykonanim akce.
        /// </summary>
        public static bool isMeetingRoomSelected(MeetingRoom meetingRoom)
        {
            if (meetingRoom == null) { MessageBox.Show("No Meeting Room selected!"); return false; }
            else return true;
        }

        /// <summary>
        /// Potvrzuje uzivateluv pozadavek na smazani <see cref="MeetingCentre"/> a jeho <see cref="List{MeetingRoom}"/>
        /// </summary>
        public static bool deleteMeetingRoomConfirm()
        {
            string msg = "Do you sure want to delete this Meeting Room?";
            MessageBoxResult result =
              MessageBox.Show(
                msg,
                "EBC Meeting Rooms Manager",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                return true;
            else return false;
        }

        /// <summary>
        /// Potvrzuje uzivateluv pozadavek na smazani <see cref="MeetingCentre"/> a jeho <see cref="List{MeetingRoom}"/>
        /// </summary>
        public static bool deleteMeetingCentreConfirm()
        {
            string msg = "Do you sure want to delete this Meeting Centre and all its Meeting Rooms?";
            MessageBoxResult result =
              MessageBox.Show(
                msg,
                "EBC Meeting Rooms Manager",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                return true;
            else return false;
        }
    }
}
