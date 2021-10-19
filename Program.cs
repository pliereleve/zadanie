using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace zadanie
{
    class Program
    {
        // Metoda log_konsola loguje komunikaty do konsoli, 
        // przyjmując za argumenty tekst komunikatu i jego typ
        // (1 - informacja, 2 - operacja powiodla sie, 3 - ostrzezenie, 4 - operacja nie powiodla sie, 5 - blad)

        static void log_konsola(string tekst, int typ)
        {
            switch(typ)
            {
                case 1:
                    Console.WriteLine("FYI: " + tekst);
                    break;
                case 2:
                    Console.WriteLine("hurra! " + tekst);
                    break;
                case 3:
                    Console.WriteLine("achtung! " + tekst);
                    break;
                case 4:
                    Console.WriteLine("cos nie pyklo.. " + tekst);
                    break;
                case 5:
                    Console.WriteLine("error: " + tekst);
                    break;
            }
        }

        // Metoda log_plik loguje komunikat do podanej przez uzytkownika sciezki,
        // przyjmując za argumenty tekst komunikatu i jego typ
        // (1 - informacja, 2 - operacja powiodla sie, 3 - ostrzezenie, 4 - operacja nie powiodla sie, 5 - blad)

        static void log_plik(string tekst, int typ)
        {
            string path;
            Console.Write("Podaj sciezke dla pliku: ");
            path = Console.ReadLine();
            if(!Directory.Exists(path))
            {
                Console.WriteLine("Podano nieprawidlowa sciezke. Sprobuj ponownie :)");
                path = Console.ReadLine();
            }

            FileStream plik = new FileStream(path + "plik_z_logami.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            byte[] tekst2;
            switch (typ)
            {
                case 1:
                    tekst2 = Encoding.UTF8.GetBytes("FYI: " + tekst + "\r\n");
                    plik.Write(tekst2, 0, tekst2.Length);
                    break;
                case 2:
                    tekst2 = Encoding.UTF8.GetBytes("hurra! " + tekst + "\r\n");
                    plik.Write(tekst2, 0, tekst2.Length);
                    break;
                case 3:
                    tekst2 = Encoding.UTF8.GetBytes("achtung! " + tekst + "\r\n");
                    plik.Write(tekst2, 0, tekst2.Length);
                    break;
                case 4:
                    tekst2 = Encoding.UTF8.GetBytes("cos nie pyklo.. " + tekst + "\r\n");
                    plik.Write(tekst2, 0, tekst2.Length);
                    break;
                case 5:
                    tekst2 = Encoding.UTF8.GetBytes("error: " + tekst + "\r\n");
                    plik.Write(tekst2, 0, tekst2.Length);
                    break;
            }
            Console.WriteLine("Plik zostal zapisany/zaktualizowany.");
            plik.Close();
        }

        // Metoda log_eventlog loguje komunikat do Event Viewera,
        // przyjmujac za argumenty tekst komunikatu i jego typ
        // (1 - informacja, 2 - operacja powiodla sie, 3 - ostrzezenie, 4 - operacja nie powiodla sie, 5 - blad)

        static void log_eventlog(string tekst, int typ)
        {
            if(!EventLog.SourceExists("zadanko"))
            {
                EventLog.CreateEventSource("zadanko", "nowy_log");
            }
            EventLog event_log = new EventLog();
            event_log.Source = "zadanko";
            switch(typ)
            {
                case 1:
                    event_log.WriteEntry(tekst, EventLogEntryType.Information, typ);
                    break;
                case 2:
                    event_log.WriteEntry(tekst, EventLogEntryType.SuccessAudit, typ);
                    break;
                case 3:
                    event_log.WriteEntry(tekst, EventLogEntryType.Warning, typ);
                    break;
                case 4:
                    event_log.WriteEntry(tekst, EventLogEntryType.FailureAudit, typ);
                    break;
                case 5:
                    event_log.WriteEntry(tekst, EventLogEntryType.Error, typ);
                    break;
            }
            Console.WriteLine("Dodano wpis do Event Viewera.");
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Program loguje podany przez uzytkownika komunikat na trzy sposoby:");
            Console.WriteLine("- do konsoli (za pomoca metody 'log_konsola'),\n- do pliku o podanej sciezce (za pomoca metody 'log_plik'),\n- do event loga (za pomoca metody 'log_eventlog').");
            Console.WriteLine("\nUWAGA: Program musi zostac uruchomiony jako administrator.\n");


            string komunikat;
            int rodzaj;
            Console.WriteLine("Podaj rodzaj komunikatu: \n1 - informacja \n2 - operacja powiodla sie \n3 - ostrzezenie \n4 - operacja nie powiodla sie \n5 - blad");
            rodzaj = int.Parse(Console.ReadLine());
            while(rodzaj!=1 && rodzaj!=2 && rodzaj!=3 && rodzaj!=4 && rodzaj!=5)
            {
                Console.WriteLine("Podano nieprawidlowa cyfre. Sprobuj ponownie :)");
                rodzaj = int.Parse(Console.ReadLine());
            }


            Console.WriteLine("Podaj komunikat do zalogowania:");
            komunikat = Console.ReadLine();
            Console.WriteLine();

            Program.log_konsola(komunikat, rodzaj);
            Console.WriteLine();

            Program.log_plik(komunikat, rodzaj);
            Console.WriteLine();

            Program.log_eventlog(komunikat, rodzaj);

            Console.ReadKey();
        }
    }
}
