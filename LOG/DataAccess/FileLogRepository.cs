using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LOG.DataAccess
{
    public class FileLogRepository : ILogRepository
    {
        private string logFilePath;

        
        public FileLogRepository()
        {
            _logFilePath = GetLogFilePath(); // Log dosyasının adını oluştur ve atama yap
        }
        private readonly string _logFilePath;


        // Log seviyeleri enum'u
        public enum LogLevel
        {
            Info,
            Error,
            Warning
        }

        public void WriteLog(LogLevel level, string message, string userName)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{level},{message},{userName}";

            using (StreamWriter writer = new StreamWriter(_logFilePath, true))
            {
                writer.WriteLine(logEntry); // Zaman damgası ile log yaz
            }
        }

        private string GetLogFilePath()
        {
            string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log"; // Tarih ve saate göre dosya ismi
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);                 
            string folderPath = Path.Combine(desktopPath, "Loglar");// Masaüstünde "Loglar" klasörünün yolunu oluştur

            // Klasör yoksa oluştur
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return Path.Combine(folderPath, fileName);
        }
    }
}
