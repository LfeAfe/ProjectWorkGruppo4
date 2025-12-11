using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWorkGruppo4
{
    internal class Logger
    {
            private const string LogDirectory = @"c:\its";
            private const string LogFileName = "log.txt";
            private static readonly string LogFilePath = Path.Combine(LogDirectory, LogFileName);
            private const string DateFormat = "dd/MM/yyyy HH:mm:ss"; // Formato richiesto 

            // Registra l'avvio del programma nel file log.txt
            public static bool LogProgramStart()
            {
                // Controllo della directory (doppio controllo per sicurezza, ma gestito anche da ConfigManager)
                if (!Directory.Exists(LogDirectory))
                {
                    Console.WriteLine("Directory del programma non presente"); // [cite: 151]
                    return false;
                }

                try
                {
                    var logEntry = DateTime.Now.ToString(DateFormat);

                    [cite_start]// Scrive su log.txt, aggiungendo (append) e non azzerando 
                    File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Errore durante la scrittura del file di log: {ex.Message}");
                    return false;
                }
            }
        }
    }

