using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOG.DataAccess;
using LOG.Network;

namespace LOG.BusinessLogic
{
    public class LogServerBusiness
    {
        private readonly ILogRepository _logRepository;
        private TcpLogServer _tcpLogServer;

        public LogServerBusiness(ILogRepository logRepository)
        {
            _logRepository = logRepository; // Veri erişim katmanını iş katmanına enjekte et
        }

        public void StartServer(int port)
        {
            _tcpLogServer = new TcpLogServer(_logRepository); // TCP sunucusu oluştur ve log veritabanını bağla
            Task.Run(() => _tcpLogServer.StartServerAsync(port)); // Sunucuyu asenkron olarak başlat
        }

        public void StopServer()
        {
            _tcpLogServer.StopServer();
        }
    }

    public class LogClientBusiness
    {
        public void SendLog(string serverIp, int port, string logMessage)
        {
            TcpLogClient client = new TcpLogClient();
            client.SendLog(serverIp, port, logMessage); // TCP üzerinden log mesajını gönder
        }
    }
}
