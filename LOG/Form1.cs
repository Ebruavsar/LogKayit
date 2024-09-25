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

            // Form yüklendiğinde güncel log dosyasını aç
            //this.Load += Form1_Load; bunu çağırınca iki kez açıyor 

            // TabControl bileşeni ekliyoruz
            //tabControlLogs = new TabControl
            //{
              //  Dock = DockStyle.Fill // TabControl'ü formun tamamına yay
           // };

            this.Controls.Add(tabControl1);
        }
        //private TabControl tabControlLogs; // Birden çok log dosyası için sekme kontrolü

        private void CustomizeTabControl(TabControl tabControl)
        {
            // Sekmelerin düz görünmesini sağla
            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.ItemSize = new Size(100, 30); // Sekme boyutları
            tabControl.DrawItem += new DrawItemEventHandler(DrawTabWithCloseButton);

            // Sekme arka planını ve yazı rengini özelleştirme
            tabControl.Padding = new Point(20, 5);



        }
        // Sekmelere kapatma simgesi ekle
        private void DrawTabWithCloseButton(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = sender as TabControl;

            // Sekmenin alanını al
            Rectangle tabRect = tabControl.GetTabRect(e.Index);

            // Sekme aktif ise arka plan rengini farklı yap
            if (e.Index == tabControl.SelectedIndex)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.LightBlue), tabRect);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.LightGray), tabRect);
            }

            // Sekme başlığını yazı ile çiz
            string tabText = tabControl.TabPages[e.Index].Text;
            Font font = new Font("Arial", 10, FontStyle.Bold);
            TextRenderer.DrawText(e.Graphics, tabText, font, tabRect, Color.Black, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            // Kapatma düğmesi eklemek için X işareti (sağ tarafa yerleştiriliyor)
            Rectangle closeButton = new Rectangle(tabRect.Right - 15, tabRect.Top , 15, 15);
            e.Graphics.DrawString("x", font, Brushes.Black, closeButton);

            // Kapatma düğmesinin sınırlarını çizmek isterseniz
            // e.Graphics.DrawRectangle(Pens.Red, closeButton);
        }

        //private void DrawTab(object sender, DrawItemEventArgs e)
        //{
        //    TabControl tabControl = sender as TabControl;

        //    // Aktif sekme ise farklı renkte çiz
        //    if (e.Index == tabControl.SelectedIndex)
        //    {
        //        e.Graphics.FillRectangle(new SolidBrush(Color.LightBlue), e.Bounds);
        //    }
        //    else
        //    {
        //        e.Graphics.FillRectangle(new SolidBrush(Color.LightGray), e.Bounds);
        //    }

        //    // Sekme başlıklarını merkezde ve kalın yazı ile yazdır
        //    string tabText = tabControl.TabPages[e.Index].Text;
        //    Font font = new Font("Arial", 10, FontStyle.Bold);
        //    TextRenderer.DrawText(e.Graphics, tabText, font, e.Bounds, Color.Black, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            LogSunucu form1 = new LogSunucu();
            {
                form1.StartPosition = FormStartPosition.CenterScreen;
                form1.Show();
            }
        }

        private void open_Click_1(object sender, EventArgs e)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Path.Combine(desktopPath, "Loglar"),
                Filter = "Text and Log files (*.txt;*.log)|*.txt;*.log",
                Title = "Bir log veya metin dosyası seçin",
                Multiselect = true // Birden çok dosya seçimi için izin ver
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] filePaths = openFileDialog.FileNames; // Seçilen dosyaları al

                foreach (string filePath in filePaths)
                {
                    try
                    {
                        AddLogFileToTab(filePath, Path.GetFileName(filePath)); // Dosya adını sekme başlığı olarak kullan
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Hata: {ex.Message}");
                    }
                }
            }


        }

        private void güncel_Click(object sender, EventArgs e)
        {
            LoadLatestLogFile();

        }

        // En güncel log dosyasını yükleyen fonksiyon
        private void LoadLatestLogFile()
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string folderPath = Path.Combine(desktopPath, "Loglar");

                if (Directory.Exists(folderPath))
                {
                    var logFiles = Directory.GetFiles(folderPath, "*.log");

                    if (logFiles.Length > 0)
                    {
                        var latestLogFile = logFiles
                            .Select(file => new FileInfo(file))
                            .OrderByDescending(fileInfo => fileInfo.LastWriteTime)
                            .FirstOrDefault();

                        if (latestLogFile != null)
                        {
                            AddLogFileToTab(latestLogFile.FullName, "Güncel Log");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }

        // Seçilen log dosyasını yeni bir sekme olarak ekleyen fonksiyon
        private void AddLogFileToTab(string filePath, string tabTitle)
        {

            var logEntries = logService.LoadLogs(filePath);

            TabPage tabPage = new TabPage
            {
                Text = tabTitle // Sekme başlığı
            };

            DataGridView logGridView = new DataGridView
            {
                DataSource = logEntries,
                Dock = DockStyle.Fill,
                ReadOnly = true
            };

            // DataGridView'i özelleştir
            CustomizeDataGridView(logGridView);

            // TabPage'e DataGridView ekle
            tabPage.Controls.Add(logGridView);
            tabControl1.TabPages.Add(tabPage);

            // TabControl stilini uygula
            CustomizeTabControl(tabControl1);
        }

        private void CustomizeDataGridView(DataGridView dataGridView)
        {
            // DataGridView stil ayarları
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dataGridView.BackgroundColor = Color.White;

            // ColumnHeader stil ayarları
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            // Otomatik sütun genişliği ayarı
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Satır başlığı ve seçilebilirlik ayarları
            dataGridView.RowHeadersVisible = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Satırların sadece okunabilir olmasını sağla
            dataGridView.ReadOnly = true;
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            LoadLatestLogFile();
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                Rectangle tabRect = tabControl1.GetTabRect(i);
                Rectangle closeButton = new Rectangle(tabRect.Right - 20, tabRect.Top + 8, 15, 15); // Kapatma simgesi alanı

                if (closeButton.Contains(e.Location))
                {
                    // Kapatma düğmesine tıklandıysa sekmeyi kaldır
                    tabControl1.TabPages.RemoveAt(i);
                    break;
                }
            }
        }
    }
}

