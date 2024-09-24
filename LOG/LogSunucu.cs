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
        private System.Timers.Timer timer;
        public LogSunucu()
        {
            InitializeComponent();
            logServerBusiness = new LogServerBusiness(new FileLogRepository()); // Veri erişim sınıfını enjekte et
            logClientBusiness = new LogClientBusiness();
            timer = new System.Timers.Timer();
            timer.Elapsed += OnTimedEvent; // Timer olayı
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

        private void button3_Click(object sender, EventArgs e)
        {
            string serverIp = textBox3.Text; // Sunucu IP'sini UI'dan al
            int port = Convert.ToInt32(textBox4.Text); // Port numarasını UI'dan al
            string logMessage = textBox1.Text; // Log mesajını UI'dan al

            timer.Interval = (double)numericUpDown1.Value * 1000; // Periyot ayarı (saniye cinsinden)
            timer.Start();

            logClientBusiness.SendPeriodikLog(serverIp, port, logMessage, timer); // Log mesajını gönder
            label1.Text = "Log mesajı gönderildi.";

        }
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            button3_Click(sender, e);
        }
    }
}
