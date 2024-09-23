using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LOG.DataAccess;

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

        private async Task HandleClientAsync(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                _logRepository.WriteLog(message); // Gelen mesajı veri erişim katmanına ileterek log dosyasına yaz
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
        public void SendLog(string serverIp, int port, string logMessage)
        {
            try
            {
                using (TcpClient client = new TcpClient(serverIp, port))
                {
                    NetworkStream stream = client.GetStream();
                    byte[] data = Encoding.UTF8.GetBytes(logMessage);
                    stream.Write(data, 0, data.Length); // Mesajı sunucuya gönder
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }
        }
    }
}
