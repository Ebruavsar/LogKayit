using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.IO;
using LOG.BusinessLogic;
using LOG.DataAccess;

namespace LOG
{
    public partial class LogSunucu : Form
    {
        public LogSunucu()
        {
            InitializeComponent();
            logServerBusiness = new LogServerBusiness(new FileLogRepository()); // Veri erişim sınıfını enjekte et
            logClientBusiness = new LogClientBusiness();
        }
        private LogServerBusiness logServerBusiness;
        private LogClientBusiness logClientBusiness;


        // Sunucuyu başlatan buton
        private void button1_Click(object sender, EventArgs e)
        {
            int port = Convert.ToInt32(textBox2.Text); // Port numarasını UI'dan al
            logServerBusiness.StartServer(port); // Sunucu başlat
            label1.Text = "Log sunucusu başlatıldı.";
        }

        // Log mesajı gönderen buton
        private void button2_Click(object sender, EventArgs e)
        {
            string serverIp = textBox3.Text; // Sunucu IP'sini UI'dan al
            int port = Convert.ToInt32(textBox4.Text); // Port numarasını UI'dan al
            string logMessage = textBox1.Text; // Log mesajını UI'dan al

            logClientBusiness.SendLog(serverIp, port, logMessage); // Log mesajını gönder
            label1.Text = "Log mesajı gönderildi.";
        }
    }
}
