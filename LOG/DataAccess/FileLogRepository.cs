using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG.DataAccess
{
    public class FileLogRepository : ILogRepository
    {
        private string logFilePath;

        public FileLogRepository()
        {
            logFilePath = GetLogFilePath(); // Log dosyasının adını oluştur
        }

        public void WriteLog(string message)
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true)) // Dosyaya ekleme modunda yaz
            {
                writer.WriteLine($"{DateTime.Now}: {message}"); // Zaman damgası ile log yaz
            }
        }

        private string GetLogFilePath()
        {
            string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log"; // Tarih ve saate göre dosya ismi
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // Dosya masaüstüne kaydedilir
            return Path.Combine(folderPath, fileName);
        }
    }
}
