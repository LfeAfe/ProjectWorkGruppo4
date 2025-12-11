using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWorkGruppo4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /* Azienda azienda = new Azienda();
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
                         try 
                         {
                             Console.Write("Inserisci l'ID dell'impiegato: ");
                             int id = int.Parse(Console.ReadLine());
                             Impiegato impiegato = azienda.ricercaPerID(id);
                             Console.WriteLine(impiegato != null ? impiegato.ToString() : "Impiegato non trovato.");
                         }
                         catch (FormatException)
                         {
                             Console.WriteLine("ID non valido. Inserisci un numero intero.");
                         }
                         break;
                         case "2":

                         Console.Write("Inserisci il NOME dell'impiegato: ");
                         string nome = Console.ReadLine();
                         Console.Write("Inserisci il COGNOME dell'impiegato: ");
                         string cognome = Console.ReadLine();
                         Impiegato impiegato2 = azienda.ricercaPerNomeCognome(nome, cognome);
                         Console.WriteLine(impiegato2 != null ? impiegato2.ToString() : "Impiegato non trovato.");
                         break;

                         case "3":


                     case "4":
                         try 
                         {
                             Console.Write("Inserisci il nome del manager: ");
                             string nomeManager = Console.ReadLine();
                             List<Impiegato> impiegatiManager = azienda.ricercaPerManager(nomeManager);
                             if (impiegatiManager.Count > 0)
                             {
                                 foreach (var imp in impiegatiManager)
                                 {
                                     Console.WriteLine(imp.ToString());
                                 }
                             }
                             else
                             {
                                 Console.WriteLine("Nessun impiegato trovato per questo manager.");
                             }
                         }
                         catch (Exception ex)
                         {
                             Console.WriteLine("Errore durante la ricerca: " + ex.Message);
                         }
                         break;
                         case "5":
                         azienda.apriFileImpiegati(@"c:\tmp\impiegati_out.txt");
                         break;
                         case "6":
                         continua = false;
                         break;
                         default:
                         Console.WriteLine("Opzione non valida. Riprova.");
                         break;
                 } 
             } 

                try
                {
                    // 1) controlla presenza directory c:\its
                    if (!Directory.Exists(BaseDir))
                    {
                        Console.WriteLine("Directory di configurazione non presente");
                        Console.WriteLine("Directory del programma non presente");
                        // non permettere proseguimento
                        return 1;
                    }

                    // registra l'avvio nel log (deve esistere directory altrimenti siamo già usciti)
                    AppendLog($"Avvio programma - {NowFormatted()}");

                    // 2) controlla presenza file config.ini
                    if (!File.Exists(ConfigPath))
                    {
                        Console.WriteLine("File di configurazione non presente");
                        AppendLog($"ERRORE - file di configurazione non presente - {NowFormatted()}");
                        return 1;
                    }

                    // 3) legge il file config.ini e cerca i parametri
                    var cfg = ReadSimpleIni(ConfigPath);

                    // Controlla DATA_SCADENZA presente
                    if (!cfg.ContainsKey("DATA_SCADENZA"))
                    {
                        Console.WriteLine("Data scadenza non presente");
                        AppendLog($"ERRORE - DATA_SCADENZA non presente - {NowFormatted()}");
                        return 1;
                    }

                    // 4) controllare che DATA_SCADENZA sia data valida
                    string dataScadenzaRaw = cfg["DATA_SCADENZA"];
                    if (!TryParseDateDdMmYyyy(dataScadenzaRaw, out DateTime dataScadenza))
                    {
                        Console.WriteLine("Errore nel file di configurazione");
                        AppendLog($"ERRORE - formato DATA_SCADENZA non valido ({dataScadenzaRaw}) - {NowFormatted()}");
                        return 1;
                    }

                    // 5) controllare se scaduta (se la data è minore di oggi -> scaduta)
                    // Nota: interpretazione più comune: se data < oggi => "Licenza scaduta"
                    if (dataScadenza.Date < DateTime.Today)
                    {
                        Console.WriteLine("Licenza scaduta");
                        AppendLog($"LICENZA SCADUTA - DATA_SCADENZA {dataScadenza.ToString("dd/MM/yyyy")} - {NowFormatted()}");
                        return 1;
                    }

                    // 6) controllare presenza PATH_CSV
                    if (!cfg.ContainsKey("PATH_CSV") || string.IsNullOrWhiteSpace(cfg["PATH_CSV"]))
                    {
                        Console.WriteLine("Path csv non presente");
                        AppendLog($"ERRORE - PATH_CSV non presente - {NowFormatted()}");
                        return 1;
                    }

                    // 7) controllare presenza PATH_OUT
                    if (!cfg.ContainsKey("PATH_OUT") || string.IsNullOrWhiteSpace(cfg["PATH_OUT"]))
                    {
                        Console.WriteLine("Path out non presente");
                        AppendLog($"ERRORE - PATH_OUT non presente - {NowFormatted()}");
                        return 1;
                    }

                    string pathCsv = cfg["PATH_CSV"];
                    string pathOut = cfg["PATH_OUT"];

                    // Controlla che il file csv esista
                    if (!File.Exists(pathCsv))
                    {
                        Console.WriteLine($"File csv non presente: {pathCsv}");
                        AppendLog($"ERRORE - file csv non trovato ({pathCsv}) - {NowFormatted()}");
                        return 1;
                    }

                    // Proviamo a leggere il csv e scrivere l'output di esempio
                    AppendLog($"Esecuzione principale - lettura {pathCsv} scrittura {pathOut} - {NowFormatted()}");

                    try
                    {
                        var lines = File.ReadAllLines(pathCsv);
                        // Esempio semplice: copiamo il CSV in PATH_OUT con una intestazione e timestamp
                        using (var sw = new StreamWriter(pathOut, false)) // sovrascrive (spec non precisa, si può cambiare)
                        {
                            sw.WriteLine($"Elaborazione eseguita il {NowFormatted()}");
                            sw.WriteLine("----- Contenuto CSV -----");
                            foreach (var l in lines) sw.WriteLine(l);
                        }

                        Console.WriteLine("Elaborazione completata con successo.");
                        AppendLog($"OK - elaborazione completata - {NowFormatted()}");
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Errore durante l'elaborazione dei file: " + ex.Message);
                        AppendLog($"ERRORE - durante elaborazione: {ex.Message} - {NowFormatted()}");
                        return 1;
                    }
                }
                catch (Exception ex)
                {
                    // evento imprevisto
                    AppendLog($"ERRORE FATALE - {ex.Message} - {NowFormatted()}");
                    Console.WriteLine("Errore imprevisto: " + ex.Message);
                    return 1;
                }
            }

            // Legge un file INI molto semplice: linee "KEY=VALUE". Ignora commenti che iniziano con ';' o '#'.
            // Restituisce un dizionario con chiavi maiuscole.
            static Dictionary<string, string> ReadSimpleIni(string path)
            {
                var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var raw in File.ReadAllLines(path))
                {
                    string line = raw.Trim();
                    if (string.IsNullOrEmpty(line)) continue;
                    if (line.StartsWith(";") || line.StartsWith("#")) continue;
                    int idx = line.IndexOf('=');
                    if (idx <= 0) continue;
                    string key = line.Substring(0, idx).Trim();
                    string value = line.Substring(idx + 1).Trim();
                    // Rimuovo possibili virgolette attorno al valore
                    if ((value.StartsWith("\"") && value.EndsWith("\"")) || (value.StartsWith("'") && value.EndsWith("'")))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }
                    dict[key] = value;
                }
                return dict;
            }

            // Parse dd/MM/yyyy (senza orario)
            static bool TryParseDateDdMmYyyy(string s, out DateTime dt)
            {
                dt = default;
                if (string.IsNullOrWhiteSpace(s)) return false;
                // rimuovo eventuali spazi
                s = s.Trim();
                // possibile che sia dd/MM/yyyy oppure dd/MM/yyyy HH:mm:ss -> gestisco entrambe
                string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "dd/MM/yyyy HH:mm:ss", "d/M/yyyy H:m:s" };
                return DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            }

            // Aggiunge una riga al log (crea il file se non esiste). Il log non deve azzerarsi ad ogni avvio.
            static void AppendLog(string message)
            {
                try
                {
                    Directory.CreateDirectory(BaseDir); // sicuro: se esiste non fa nulla
                    using (var sw = new StreamWriter(LogPath, true))
                    {
                        sw.WriteLine($"{NowFormatted()} - {message}");
                    }
                }
                catch
                {
                    // non fare nulla: non vogliamo lanciare eccezioni di log
                }
            }

            static string NowFormatted()
            {
                return DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            }
        } */

            Console.WriteLine("Avvio programma...");

            // 1. Caricamento e Validazione Configurazione
            var config = new conf();
            if (!config.LoadAndValidateConfig())
            {
                Console.WriteLine("Avvio interrotto per problemi di configurazione.");
                return;
            }

            // 2. Logging dell'avvio
            if (!Logger.LogProgramStart())
            {
                Console.WriteLine("Avvio interrotto per problemi di logging.");
                return;
            }

            // A questo punto, la configurazione è valida e l'avvio è stato loggato.
            Console.WriteLine("Configurazione OK e avvio loggato con successo.");
            Console.WriteLine($"Data di scadenza (licenza): {config.DataScadenza.ToShortDateString()}");
            Console.WriteLine($"Path CSV: {config.PathCsv}");
            Console.WriteLine($"Path OUT: {config.PathOut}");



        }

    }

}


