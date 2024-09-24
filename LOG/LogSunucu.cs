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
using static LOG.DataAccess.FileLogRepository;


namespace LOG
{
    public partial class LogSunucu : Form
    {
        private System.Timers.Timer timer;
        private readonly ILogRepository _logRepository;
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
            string userName = textBox5.Text;

            // ComboBox'tan seçilen log seviyesini al ve enum'a dönüştür
            LogLevel level;
            if (Enum.TryParse(comboBox1.SelectedItem.ToString(), out level))
            {
                logClientBusiness.SendLog(serverIp, port, level, logMessage, userName);// Log mesajını gönder
                // Log'u `logServerBusiness` üzerinden gönder
                //logServerBusiness.WriteLog(level, logMessage, userName);
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir log seviyesi seçin.");
            }

             
            label1.Text = "Log mesajı gönderildi.";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string serverIp = textBox3.Text; // Sunucu IP'sini UI'dan al
            int port = Convert.ToInt32(textBox4.Text); // Port numarasını UI'dan al
            string logMessage = textBox1.Text; // Log mesajını UI'dan al
            string userName = textBox5.Text;

            // ComboBox'tan seçilen log seviyesini al ve enum'a dönüştür
            LogLevel level;

            timer.Interval = (double)numericUpDown1.Value * 1000; // Periyot ayarı (saniye cinsinden)
            timer.Start();
            if (Enum.TryParse(comboBox1.SelectedItem.ToString(), out level))
            {
                // Log'u `logServerBusiness` üzerinden gönder
                logClientBusiness.SendPeriodikLog(serverIp, port, level, logMessage, userName, timer); // Log mesajını gönder

            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir log seviyesi seçin.");
            }


            label1.Text = "Log mesajı gönderiliyor..";

        }
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            // UI işlemi olduğundan, bunu ana thread'de çalıştırmamız gerekiyor.
            if (InvokeRequired)
            {
                // Eğer UI thread'inde değilsek, Invoke kullanarak UI thread'ine geçelim.
                this.Invoke(new Action(() => button3_Click(sender, e)));
            }
            else
            {
                // UI thread'indeysek doğrudan çalıştırabiliriz.
                button3_Click(sender, e);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            logServerBusiness.StopServer();
            label1.Text = "Log sunucusu bağlantısı kesildi.";

        }
    }
}
