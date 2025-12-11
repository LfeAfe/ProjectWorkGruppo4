using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWorkGruppo4
{
    internal class conf
    {

        // Percorso fisso per la directory e il file di configurazione
        private const string ConfigDirectory = @"c:\its";
        private const string ConfigFileName = "config.ini";
        private string ConfigFilePath = Path.Combine(ConfigDirectory, ConfigFileName);

        // Proprietà per contenere i parametri letti
        public string DataScadenzaStr { get; private set; }
        public DateTime DataScadenza { get; private set; }
        public string PathCsv { get; private set; }
        public string PathOut { get; private set; }

        // Dictionary per memorizzare tutti i parametri letti dal file .ini
        private readonly Dictionary<string, string> _configParams = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Metodo principale per caricare e validare la configurazione
        public bool LoadAndValidateConfig()
        {
            // 1. Controllo directory (c:\its)
            if (!Directory.Exists(ConfigDirectory))
            {
                Console.WriteLine("Directory di configurazione non presente"); // 
                return false;
            }

            // 2. Controllo file config.ini
            if (!File.Exists(ConfigFilePath))
            {
                Console.WriteLine("File di configurazione non presente"); // [cite: 144]
                return false;
            }

            // Legge e analizza il contenuto del file config.ini
            if (!ReadConfigFile())
            {
                return false;
            }

            // Esegue le validazioni specifiche sui parametri richiesti

            // 3. Controllo parametro DATA_SCADENZA
            if (!_configParams.ContainsKey("DATA_SCADENZA"))
            {
                Console.WriteLine("Data scadenza non presente"); // [cite: 145]
                return false;
            }
            DataScadenzaStr = _configParams["DATA_SCADENZA"];

            // 4. Controllo validità DATA_SCADENZA
            if (!DateTime.TryParse(DataScadenzaStr, out DateTime scadenza))
            {
                Console.WriteLine("Errore nel file di configurazione (DATA_SCADENZA non valida)"); // [cite: 146]
                return false;
            }
            DataScadenza = scadenza;

            // 5. Controllo scadenza licenza
            if (DataScadenza < DateTime.Today) // Se la data è passata, la licenza è scaduta
            {
                Console.WriteLine("Licenza scaduta"); // [cite: 147]
                return false;
            }

            // 6. Controllo parametro PATH_CSV
            if (!_configParams.ContainsKey("PATH_CSV"))
            {
                Console.WriteLine("Path csv non presente"); // [cite: 148]
                return false;
            }
            PathCsv = _configParams["PATH_CSV"];

            // 7. Controllo parametro PATH_OUT
            if (!_configParams.ContainsKey("PATH_OUT"))
            {
                Console.WriteLine("Path out non presente"); // [cite: 149]
                return false;
            }
            PathOut = _configParams["PATH_OUT"];

            return true; // Configurazione caricata e valida
        }

        // Metodo per leggere le coppie chiave=valore dal file .ini
        private bool ReadConfigFile()
        {
            try
            {
                var lines = File.ReadAllLines(ConfigFilePath);
                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();
                    if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                    {
                        continue; // Salta commenti e linee vuote
                    }

                    // Cerca il primo '=' per dividere chiave e valore
                    var separatorIndex = trimmedLine.IndexOf('=');
                    if (separatorIndex > 0)
                    {
                        var key = trimmedLine.Substring(0, separatorIndex).Trim();
                        var value = trimmedLine.Substring(separatorIndex + 1).Trim();
                        _configParams[key] = value;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante la lettura di {ConfigFileName}: {ex.Message}");
                return false;
            }
        }
    }
}
