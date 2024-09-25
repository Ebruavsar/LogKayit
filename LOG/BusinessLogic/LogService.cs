using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LOG.DataAccess.FileLogRepository;

namespace LOG.BusinessLogic
{
    public class LogService
    {

        public List<LogEntry> LoadLogs(string filePath)
        {
            var logEntries = new List<LogEntry>();

            string[] logLines = File.ReadAllLines(filePath);

            foreach (string line in logLines)
            {
                string[] logParts = line.Split(',');

                if (logParts.Length == 4)
                {
                    DateTime logTime;
                    if (DateTime.TryParse(logParts[0], out logTime))
                    {
                        var logEntry = new LogEntry
                        {
                            Tarih = logTime,
                            Severity = logParts[1],
                            Message = logParts[2],
                            KullaniciAdi = logParts[3]
                        };

                        logEntries.Add(logEntry);
                    }
                    else
                    {
                        throw new Exception($"Geçersiz tarih: {logParts[0]}");
                    }
                }
                else
                {
                    throw new Exception($"Hatalı satır: {line}");
                }
            }

            return logEntries; // Yüklenen logları LogEntry listesi olarak döndür
        }
    }
}
