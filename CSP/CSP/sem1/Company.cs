﻿using System;
using System.Collections.Generic;
using System.Linq;
namespace XA01
{
    public enum CompanyState
    {
        Idle,
        Running,
        Bankrupt,
        Finished
    }

    public class Company
    {
        public string Name { get; private set; }
        public int Capacity { get; private set; }
        public int DailyExpenses { get; private set; }
        public int Budget { get; private set; }
        public int Days { get; private set; } = 0;
        public CompanyState State { get; set; } = CompanyState.Idle;
        public List<Programmer> Programmers { get; private set; } = new List<Programmer>();
        public List<Project> ProjectsWaiting { get; private set; } = new List<Project>();
        public List<Project> ProjectsCurrent { get; private set; } = new List<Project>();
        public List<Project> ProjectsDone { get; private set; } = new List<Project>();

        private Logger logger;

        public Company(string name, int capacity,int dailyExpenses, int budget)
        {
            Name = name;
            Capacity = capacity;
            DailyExpenses = dailyExpenses;
            Budget = budget;
            // IMPLEMENTUJTE ZDE: az budete mit implementovanu tridu Logger, pak zde ziskejte jeji instanci a ulozte
            // ji do promenne logger.
            logger = Logger.getLoggerInstance;
        }

        /// <summary>
        /// Nacte vsechny projekty do kolekce ProjectsWaiting
        /// </summary>
        public void AllocateProjects(List<Project> projects)
        {
            foreach(Project proj in projects)
            {
                ProjectsWaiting.Add(proj);
            }
        }

        /// <summary>
        /// Najme tolik programatoru kolik cini kapacita firmy.
        /// IMPLEMENTUJTE TUDO METODU
        ///     Z parametru programmers vyberte prvnich Capacity programatoru 
        ///     v poradi podle nejvyhodnejsiho pomeru jejich rychlosti k jejich cene.
        ///     V tomto poradi je vlozte do kolekce Programmers.
        ///     
        ///     Poznamka k trideni: toto je mozne implementovat vice zpusoby. Vyberte si libovolny, s tim
        ///         ze je povoleno kvuli tomuto trideni napr. vytvorit novou tridu nebo modifikovat nejakou stavajici.
        /// </summary>
        public void AllocateProgrammers(List<Programmer> programmers)
        {
            List<Programmer> sortedProgrammers = programmers.OrderByDescending(p => p.Speed / p.DailyWage).ToList(); //ulozeni do "docasneho" Listu za ziskem uspory vypocetniho casu (nez treba tridit pri kazde iteraci cyklu)
          
            for (int i = 0; i < Capacity; i++)
                Programmers.Add(sortedProgrammers[i]);
        }

        /// <summary>
        /// IMPLEMENTUJTE TUDO METODU
        ///     Z ProjectsCurrent odeberte ty projekty, ktere jsou dokoncene. Dokonceny projekt je ten, ktery ma ManDaysDone >= ManDays.
        ///     Zaroven temto projektum nastavte spravny stav, pridejte je do ProjectsDone a do rozpoctu firmy prictete utrzene penize za projekt.
        /// </summary>
        public void CheckProjects()
        {
            //hodne jsem se tu snazil prijit na "elegantnejsi" implementaci, ale nic me nenapadlo... :D
            for (int i = 0; i < ProjectsCurrent.Count; i++)
                if(ProjectsCurrent[i].ManDaysDone >= ProjectsCurrent[i].ManDays)
                {
                    ProjectsCurrent[i].State = ProjectState.Done;
                    ProjectsDone.Add(ProjectsCurrent[i]);
                    Budget += ProjectsCurrent[i].Price;
                    ProjectsCurrent.Remove(ProjectsCurrent[i]);
                }
        }

        /// <summary>
        /// IMPLEMENTUJTE TUTO METODU
        ///     Uvolnete programatory pracujici na projektech, ktere jsou dokonceny.
        /// </summary>
        public void CheckProgrammers()
        {
            Programmers.ForEach(p => { if (p.Project.State == ProjectState.Done) p.ClearProject(); }); //linq iterace Programmers a jejich pripadne uvolneni
        }

        /// <summary>
        /// IMPLEMENTUJTE TUTO METODU
        ///     Nastavte projekty programatorum, kteri aktualne nepracuji na zadnem projektu.
        ///     Pro kazdeho volneho programatora hledejte projekt k prirazeni nasledujicim zpusobem:
        ///         - Pokud existuje nejaky projekt v ProjectsWaiting, vyberte prvni takovy.
        ///           Nezapomente mu zmenit stav a presunout jej do ProjectsCurrent.
        ///         - Pokud ne, vyberte takovy projekt z ProjectsCurrent, na kterem zbyva nejvice
        ///           nedodelane prace.
        /// </summary>
        public void AssignNewProjects()
        {
            foreach (Programmer programmer in Programmers)
                if (programmer.Project == null)
                    if (ProjectsWaiting.Count != 0)
                    {
                        ProjectsWaiting.First().State = ProjectState.Current;
                        programmer.AssignProject(ProjectsWaiting.First());
                        ProjectsCurrent.Add(ProjectsWaiting.First());
                        ProjectsWaiting.Remove(ProjectsWaiting.First());
                    }
                    else
                        programmer.AssignProject(ProjectsCurrent.OrderByDescending(p => (p.ManDays - p.ManDaysDone)).ToList().First()); //linq setrizeni nejmene dokoncenych projektu z ProjectsCurrent a prirazeni prvniho z nich
        }

        /// <summary>
        /// IMPLEMENTUJTE TUTO METODU
        ///     Projdete vsechny programatory a predejte jejich denni vykon(praci) projektum (metoda  WriteCode).
        ///     Zaroven snizte aktualni stav financi firmy o jejich denni mzdu a rovnez o denni vydaje firmy.
        /// </summary>
        public void ProgrammersWork()
        {
            Programmers.ForEach(x => { x.WriteCode(); Budget -= x.DailyWage; }); //linq iterace Programmers a predani bloku na vykonani prace a odecteni dennich vyplat programatoru
            Budget -= DailyExpenses;
        }

        /// <summary>
        /// IMPLEMENTUJTE TUTO METODU
        ///     Pokud je aktualni stav financi firmy zaporny, nastavte stav firmy na Bankrupt.
        ///     Pokud ne a zaroven pokud jsou jiz vsechny projekty hotove, nastavte stav firmy na Finished.
        /// </summary>
        public void CheckCompanyState()
        {
            if (Budget < 0)
                State = CompanyState.Bankrupt;
            else if (ProjectsWaiting.Count == 0 && ProjectsCurrent.Count == 0)
                State = CompanyState.Finished;
        }

        /// <summary>
        /// Spusteni simulace. Simulace je ukoncena pokud je stav firmy
        /// Bankrupt nebo Finished, nebo pokud simulace bezi dele nez 1000 dni.
        /// </summary>
        public void Run()
        {
            // IMPLEMENTUJTE ZDE: az budete mit implementovanu tridu Logger, pak nasledujici radek odkomentujte
            logger.Log(string.Format("Company: {0}, started with budget: {1}", Name, Budget));
            State = CompanyState.Running;
            while(State != CompanyState.Bankrupt && State != CompanyState.Finished && Days <= 1000)
            {
                Days++;
                AssignNewProjects();
                ProgrammersWork();
                CheckProjects();
                CheckProgrammers();
                CheckCompanyState();
            }
            if (State == CompanyState.Running)
                State = CompanyState.Idle;
        }

        public void PrintResult()
        {
            Console.WriteLine("\nCompany name: {0}",Name);
            Console.WriteLine("Days running: {0}",Days);
            Console.WriteLine("Final budget: {0}", Budget);
            Console.WriteLine("Final state: {0}", State);
            Console.WriteLine("Count of projects done: {0}", ProjectsDone.Count);
        }
    }
}
