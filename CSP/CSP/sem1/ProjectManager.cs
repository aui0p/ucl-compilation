using System;
using System.Collections.Generic;

namespace XA01
{
    /// <summary>
    /// IMPLEMENTUJTE ZDE
    ///     Tato trida zatim neni v programu nikde vyuzita, ale predstavte si, ze program bude nekdy v budoucnu dale rozvijen.
    ///     Trida ProjectManager ma nektere prvky spolecne s tridou Programmer. Vytvorte vhodneho spolecneho predka temto tridam,
    ///     o kterem vite, ze bude obecny a nebudou z nej v programu vytvareny instance. Timto tedy musite upravit jak tuto tridu tak
    ///     tridu Programmer, pricemz ze spolecneho predka vyuzijte jeho konstruktor v potomcich.
    /// </summary>
    public class ProjectManager : Employee
    {
        public List<Project> ManagedProjects { get; set; } 

        public ProjectManager(string name, int dailywage, List<Project> managedProjects) : base(name, dailywage) 
        {
            Name = name;
            DailyWage = dailywage;
            ManagedProjects = managedProjects;
        }
        
        public override void doWork()
        {
           //viz Employee class
        }





        //chapu myslenku primeho pristupu k ManagedProjects, avsak je diskutabilni, jestli nebude efektivnejsi private setter, setovaci metodu a odstranit inicializaci Listu z konstruktoru - budeme vzdy znat manazerovy projekty jiz pri jeho instanciovani? (narazim tim na stejnou architekturu u tridy Programmer)

        //public List<Project> NanagedProjects { get; private set; }

        //public ProjectManager(string name, int dailywage) : base(name, dailywage)
        //{
        //    Name = name;
        //    DailyWage = dailywage;
        //}

        //public void assignProject(Project project)
        //{
        //    ManagedProjects.Add(project);
        //}
    }
}
