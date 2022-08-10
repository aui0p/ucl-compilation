using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EntityGarbageWPF.Util
{
    public static class ContentChecker
    {
        /// <summary>
        /// Kontroluje, jestli uzivatel vybral <see cref="Accessory"/> z <see cref="MainWindow.dataGrid"/> pred vykonanim akce.
        /// </summary>
        public static bool isSelectedAccessory(Accessory accessory)
        {
            if (accessory == null) { MessageBox.Show("No Accessory selected!"); return false; }
            else return true;
        }

       

        /// <summary>
        /// Potvrzuje uzivateluv pozadavek na smazani <see cref="Accessory"/>
        /// </summary>
        public static bool deleteAccessoryConfirm()
        {
            string msg = "Do you sure want to delete this Accessory?";
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
        /// Zkontroluje, jestli neni <param name="s"></param> prazdny, pripadne upozorni uzivatele
        /// </summary>
        public static bool stringIsNotEmpty(string s)
        {
            if (s == null || s.Length == 0) { MessageBox.Show("Name cannot be empty!"); return false; }
            else return true;
        }
    }
}
