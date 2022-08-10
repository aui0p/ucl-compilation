using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XA01
{
    public abstract class Employee
    {
        public string Name { get; protected set; } //protected settery z duvodu "privatniho" prirazovani hodnot
        public int DailyWage { get; protected set; }
        public double Speed { get; protected set; }

        //konstruktory nechavam prazdne a samotnou inicializaci promennych provadim az v potomcich, nevim, jestli je to architektonicky spravne, ale prijde mi to logictejsi z hlediska prehlednosti toho, co delam s potomky
        public Employee(string name, int dailywage, double speed)
        {
           
        }
        //pomerne konkretni pripad pro nas ukol, konstruktor pro ProjectManagera s absenci parametru Speed
        public Employee(string name, int dailywage)
        {

        }

        public abstract void doWork(); //pouze napad, kazdy zamestnanec pracuje - moznost budouci implementace u ProjectManagera a reimplementace u Programatora, kdy WriteCode by se stala privatni metodou a prezentovala by ji ven prave tato verejna metoda doWork()
    }
}
