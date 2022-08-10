using System;

namespace XA01
{
    /// <summary>
    /// IMPLEMENTUJTE ZDE
    ///     Upravte tuto tridu tak, aby v programu byla vzdy prave jedna a ta sama instance. 
    ///     Jde o implementaci podle navrhoveho vzoru Singleton.
    ///     Cilem teto tridy je poskytnout moznost zapisovani logovacich informaci. Zatim neumime
    ///     pracovat se soubory nebo napr. s databazi, proto je logovani simulovano pouhym vypisem do konzole.
    /// </summary>
    public sealed class Logger //sealed zajistuje unikatnost (jedinecnost tridy) - nelze od ni dedit
    {
        private static readonly Logger loggerInstance = new Logger(); //privatni readonly instance - tim je zajisteno, ze bude alkovana jen jednou
        
        private Logger() //privatni konstruktor - trida pak muze alokovat jen sebe
        {
        }

        public static Logger getLoggerInstance //public static getter pro ziskani pristupu k instanci
        {
            get
            {
                return loggerInstance;
            }
        }
        public void Log(string message)
        {
            Console.WriteLine("Logger: {0}", message);
        }
    }
}
