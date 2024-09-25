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
        

        public DataTable LoadLogs(string filePath)
        {
            var logTable = new DataTable();
            logTable.Columns.Add("Tarih", typeof(DateTime));
            logTable.Columns.Add("Level", typeof(string));
            logTable.Columns.Add("Mesaj", typeof(string));
            logTable.Columns.Add("Kullanıcı Adı", typeof(string));

            string[] logLines = File.ReadAllLines(filePath);

            foreach (string line in logLines)
            {
                string[] logParts = line.Split(',');
                if (logParts.Length == 4)
                {
                    DateTime logTime;
                    if (DateTime.TryParse(logParts[0], out logTime))
                    {
                        logTable.Rows.Add(logTime, logParts[1], logParts[2], logParts[3]);
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

            return logTable; // Yüklenen logları döndür
        }
    }
}
