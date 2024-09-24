using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG.BusinessLogic
{
    public class LogService
    {
        

        public DataTable LoadLogs(string filePath)
        {
            var logTable = new DataTable();
            logTable.Columns.Add("Tarih", typeof(DateTime));
            logTable.Columns.Add("Tür", typeof(string));
            logTable.Columns.Add("Mesaj", typeof(string));

            string[] logLines = File.ReadAllLines(filePath);

            foreach (string line in logLines)
            {
                string[] logParts = line.Split(',');
                if (logParts.Length == 3)
                {
                    DateTime logTime;
                    if (DateTime.TryParse(logParts[0], out logTime))
                    {
                        logTable.Rows.Add(logTime, logParts[1], logParts[2]);
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
