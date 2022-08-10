using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace BudinskyMartin_Semestralni1CSP.Util
{
    public class InputValidator 
    {
        //validace je velmi trivialni - pouze vraci bool na zaklade splneni/nesplneni vsech podminek, a souhrn co ma jak byt, dalo by se podstatne vylepsit (resp. zkonkretizovat)...
        private Regex regex = new Regex(@"^[A-Za-z0-9_:.-]+$"); //hint - dle zadani by nefungoval zadny defaultni kod, protoze validace nema podporovat pomlcku a cisla :)

        /// <summary>
        /// Metoda zkontroluje dany <param name="m"></param> a na zaklade spravnosti vsech pravidel vrati true, nebo false.
        /// </summary>
        public bool Validate(MeetingCentre m)
        {
            if ((m.name.Length >= 2 && m.name.Length <= 100) && (m.code.Length >= 5 && m.code.Length <= 50 && regex.IsMatch(m.code)) && (m.description.Length >= 10 && m.description.Length <= 300))
                return true;
            else { MessageBox.Show("Validation is not correct!\nName must be 2..100 characters long\nCode must be 5..50 characters (or numbers) long and can contain . : - _\nDescription must be 10..30 characters long"); return false; }
        }

        /// <summary>
        /// Metoda zkontroluje dany <param name="m"></param> a na zaklade spravnosti vsech pravidel vrati true, nebo false.
        /// </summary>
        public bool Validate(MeetingRoom m)
        {
            if ((m.name.Length >= 2 && m.name.Length <= 100) && (m.code.Length >= 5 && m.code.Length <= 50 && regex.IsMatch(m.code)) && (m.description.Length >= 10 && m.description.Length <= 300) && (m.capacity >= 1 && m.capacity <= 100))
                return true;
            else { MessageBox.Show("Validation is not correct!\nName must be 2..100 characters long\nCode must be 5..50 characters (or numbers) long and can contain . : - _\nDescription must be 10..30 characters long\nCapacity must be > 1 and <= 100");  return false; }
        }

        /// <summary>
        /// Metoda zkontroluje dany <param name="input"></param> a na zaklade spravnosti (je-li cislo) vrati true, nebo false.
        /// </summary>
        public bool isNumber(string input)
        {
            int value;
            if (int.TryParse(input, out value)) return true;
            else { MessageBox.Show("Capacity must be a real number!"); return false; }
        }

        /// <summary>
        /// Metoda proiteruje <param name="allMeetingCentres"></param> a overi, zda-li <param name="meetingCentre"><see cref="MeetingCentre.code"/></param> je unikatni, na zaklade tohoto vrati true, nebo false.
        /// </summary>
        public bool isCodeUnique(MeetingCentre meetingCentre, List<MeetingCentre> allMeetingCentres)
        {
            if (meetingCentre.code == meetingCentre.previousCode) return true; //code nebyl editován
            foreach (var m in allMeetingCentres)
            {
                if (meetingCentre.code == m.code) { MessageBox.Show("Meeting centre with code: "+meetingCentre.code+" already exists! Choose another code!"); return false; }
            }
            return true;
        }

        /// <summary>
        /// Metoda proiteruje <param name="meetingCentre"><see cref="MeetingCentre.meetingRooms"/></param> a overi, zda-li <param name="meetingRoom"><see cref="MeetingCentre.code"/></param> je unikatni, na zaklade tohoto vrati true, nebo false.
        /// </summary>
        public bool isCodeUnique(MeetingRoom meetingRoom, MeetingCentre meetingCentre)
        {
            if (meetingRoom.code == meetingRoom.previousCode) return true; //code nebyl editován
            foreach (var m in meetingCentre.meetingRooms)
            {
                if (meetingRoom.code == m.code) { MessageBox.Show("Meeting room with code: " + meetingRoom.code + " already exists in "+ meetingCentre.name +"! Choose another code!"); return false; }
            }
            return true;
        }
    }
    

    //puvodni myslenka - mit pro kazdou validaci tridu zvlast, obe by dedily od tohoto generic interface
    //interface IValidator<T>
    //{
    //   bool Validate(T t);
    //}
}
