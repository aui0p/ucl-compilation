using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace EntityGarbageWPF.Util
{
    public static class Validator
    {
        /// <summary>
        /// Metoda zkontroluje dany <param name="input"></param> a na zaklade spravnosti (je-li cislo) vrati true, nebo false.
        /// </summary>
        public static bool isNumber(string input)
        {
            int value;
            if (int.TryParse(input, out value)) return true;
            else { MessageBox.Show("Amount must be a real number!"); return false; }
        }
        
        /// <summary>
        /// Metoda zkontroluje dany <param name="m"></param> a na zaklade spravnosti vsech pravidel vrati true, nebo false.
        /// </summary>
        public static bool Validate(Accessory a)
        {
            if ((a.Name.Length >= 2 && a.Name.Length <= 100) && (a.MinAmount >= 0 && a.MinAmount <= 1000))
                return true;
            else { MessageBox.Show("Validation is not correct!\nName must be 2..100 characters long\nMinimal amount must be 0..1000!"); return false; }
        }

        /// <summary>
        /// Metoda zkontroluje dany <param name="customer"></param> a na zaklade spravnosti vsech pravidel vrati true, nebo false.
        /// </summary>
        public static bool Validate(string customer)
        {
            if (customer.Length >= 2 && customer.Length <= 100)
                return true;
            else { MessageBox.Show("Validation is not correct!\nName must be 2..100 characters long!"); return false; }
        }

        /// <summary>
        /// Metoda zkontroluje cislo <param name="i"></param> a urcite, jestli je stale vetsi, nez 0.
        /// </summary>
        public static bool isBiggerThanZero(int i)
        {
            if (i < 0) { MessageBox.Show("Given operation is already lower than zero!"); return false; }
            else return true;
        }

        /// <summary>
        /// Metoda zkontroluje cislo <param name="i"></param> a urcite, jestli je stale mensi, nez maximalni pocet na sklade (1000).
        /// </summary>
        public static bool isNotOverloaded(int i)
        {
            if (i > 1000) { MessageBox.Show("Given operation is already greater than stock space!"); return false; }
            else return true;
        }
    }
}
