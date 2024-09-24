using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LOG.DataAccess;
using static LOG.DataAccess.FileLogRepository;

namespace LOG.Network
{
    public class TcpLogServer
    {
        private TcpListener _server;
        private readonly ILogRepository _logRepository;

        public TcpLogServer(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task StartServerAsync(int port)
        {
            _server = new TcpListener(IPAddress.Any, port); // Sunucu tüm IP'leri dinler
            _server.Start();
            Console.WriteLine($"Sunucu başlatıldı: Port {port}");

            while (true)
            {
                TcpClient client = await _server.AcceptTcpClientAsync();
                _ = HandleClientAsync(client); // Gelen istemcileri işleyip log dosyasına yaz
            }

        }



        // TCP istemcisi ile gelen log mesajını işleyelim
        private async Task HandleClientAsync(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                // Mesajı virgülle ayır
                string[] logParts = message.Split(',');

                if (logParts.Length == 4) // Eğer 4 parça varsa
                {
                    // DateTime logTime = DateTime.Parse(logParts[0]); // Tarih
                    LogLevel level = (LogLevel)Enum.Parse(typeof(LogLevel), logParts[1]); // Log seviyesi
                    string logMessage = logParts[2]; // Mesaj
                    string userName = logParts[3]; // Kullanıcı adı

                    // Log kaydını yaz
                    _logRepository.WriteLog(level, logMessage, userName);
                }
                else
                {
                    Console.WriteLine($"Geçersiz log formati: {message}");
                }
            }

            client.Close();
        }


        public void StopServer()
        {
            _server.Stop();
            Console.WriteLine("Sunucu durduruldu.");
        }
    }
    public class TcpLogClient
    {
        public void SendLog(string serverIp, int port, LogLevel level, string logMessage, string userName)
        {
            try
            {
                using (TcpClient client = new TcpClient(serverIp, port))
                {
                    NetworkStream stream = client.GetStream();

                    // Log mesajını formatlayın
                    string formattedMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{level},{logMessage},{userName}";

                    // Formatlanmış mesajı UTF8 ile byte dizisine çevirin
                    byte[] data = Encoding.UTF8.GetBytes(formattedMessage);

                    // Mesajı sunucuya gönder
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }
        }

        public void SendPeriodikLog(string serverIp, int port, LogLevel level, string logMessage, string userName, System.Timers.Timer timer)
        {
            try
            {
                using (TcpClient client = new TcpClient(serverIp, port))
                {
                    NetworkStream stream = client.GetStream();

                    // Log mesajını formatlayın
                    string formattedMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{level},{logMessage},{userName}";

                    // Formatlanmış mesajı UTF8 ile byte dizisine çevirin
                    byte[] data = Encoding.UTF8.GetBytes(formattedMessage);

                    // Mesajı sunucuya gönder
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }

        }
    }
}
