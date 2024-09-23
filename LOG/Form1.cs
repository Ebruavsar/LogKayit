using LOG.BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LOG
{
    public partial class Form1 : Form
    {
        private LogService logService; // İş mantığı katmanındaki sınıf

        public Form1()
        {
            InitializeComponent();
            logService = new LogService(); // LogService nesnesi oluşturuluyor
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text and Log files (*.txt;*.log)|*.txt;*.log",
                Title = "Bir log veya metin dosyası seçin"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    var logEntries = logService.LoadLogs(filePath); // İş mantığı katmanına çağrı yapılıyor
                    dataGridView1.DataSource = logEntries; // DataGridView'e veriler bağlanıyor
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            LogSunucu form1 = new LogSunucu();
            {
                form1.StartPosition = FormStartPosition.CenterScreen;
                form1.Show();
            }
        }
    }
}

