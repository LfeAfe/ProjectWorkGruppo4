using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWorkGruppo4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Azienda azienda = new Azienda;
            azienda.caricaImpiegati(@"c:\tmp\impiegati_out.txt");


            bool continua = true;
            while (continua)
            {

                Console.WriteLine("\nIMPIEGATI:");
                Console.WriteLine("1 - Ricerca tramite ID");
                Console.WriteLine("2 - Ricerca tramite NOME e COGNOME");
                Console.WriteLine("3 - Numero Aumenti");
                Console.WriteLine("4 - Ricerca per Manager");
                Console.WriteLine("5 - Apertura file");
                Console.WriteLine("6 - Esci");
                Console.Write("Scegli un'opzione: ");

                string scelta = Console.ReadLine();

                switch (scelta)
                {
                    case "1":
                        Console.Write("Inserisci l'ID dell'impiegato: ");
                        int id = int.Parse(Console.ReadLine());
                        Impiegato impiegatoById = azienda.impiegatoList.FirstOrDefault(i => i.getIdImpiegato() == id);
                        if (impiegatoById != null)
                        {
                            Console.WriteLine(impiegatoById);
                        }
                        else
                        {
                            Console.WriteLine("Impiegato non trovato.");
                        }
                        break;

                        case "2":
                            Console.Write("Inserisci il NOME dell'impiegato: ");
                            string nome = Console.ReadLine();
                            
                            Console.Write("Inserisci il COGNOME dell'impiegato: ");
                            string cognome = Console.ReadLine();
                            Impiegato impiegatoByName = azienda.impiegatoList.FirstOrDefault(i => i.getNome().Equals(nome, StringComparison.OrdinalIgnoreCase) && i.getCognome().Equals(cognome, StringComparison.OrdinalIgnoreCase));
                            if (impiegatoByName != null)
                            {
                                Console.WriteLine(impiegatoByName);
                            }
                            else
                            {
                                Console.WriteLine("Impiegato non trovato.");
                        }
                            break;

                    case "6":
                        continua = false;
                        Console.WriteLine("Uscita dal programma...");
                        break;

                    default:
                        Console.WriteLine("Opzione non valida!");
                        break;
                }
            }
        }
    }
}
