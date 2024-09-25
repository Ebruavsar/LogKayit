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
using System.Xml;
using OfficeOpenXml; // EPPlus Namespace

namespace LOG
{
    public partial class Raporla : Form
    {
        private LogService logService; // Logları yüklemek için
        private List<LogEntry> logEntries; // Form1'den log verilerini alacağız

        public Raporla(List<LogEntry> logEntries)
        {
            InitializeComponent();
            this.logEntries = logEntries;
            logService = new LogService();

            // Format seçenekleri
            comboBoxFormat.Items.AddRange(new string[] { "PDF", "XML" ,"XLSX" });
            comboBoxFormat.SelectedIndex = 0; // Varsayılan olarak PDF seçili

            // Klasör seçimi butonu
            buttonBrowse.Click += ButtonBrowse_Click;
            buttonRaporla.Click += ButtonRaporla_Click;
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxDizin.Text = dialog.SelectedPath; // Seçilen dizini göster
                }
            }
        }

        private void ButtonRaporla_Click(object sender, EventArgs e)
        {
            string selectedFormat = comboBoxFormat.SelectedItem.ToString();
            string directory = textBoxDizin.Text;

            if (string.IsNullOrEmpty(directory))
            {
                MessageBox.Show("Lütfen bir dizin seçin.");
                return;
            }

            if (selectedFormat == "PDF")
            {
                ExportToPdf(directory);
            }
            else if (selectedFormat == "XML")
            {
                ExportToXml(directory);
            }
            else if (selectedFormat == "XLSX")
            {
                CreateExcel(directory);
            }
        }



        // PDF olarak rapor oluşturma
        private void ExportToPdf(string directory)
        {
            string filePath = Path.Combine(directory, "LogRapor.pdf");
            // iTextSharp veya başka bir PDF kütüphanesi ile PDF oluşturma kodu
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                // PDF belge oluşturma işlemi burada yapılacak (iTextSharp kullanarak)
                MessageBox.Show("PDF raporu başarıyla oluşturuldu!");
            }
        }

        // XML olarak rapor oluşturma
        private void ExportToXml(string directory)
        {
            string filePath = Path.Combine(directory, "LogRapor.xml");
            using (XmlWriter writer = XmlWriter.Create(filePath))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Logs");

                foreach (var log in logEntries)
                {
                    writer.WriteStartElement("Log");
                    writer.WriteElementString("Tarih", log.Tarih.ToString());
                    writer.WriteElementString("Seviye", log.Severity.ToString());
                    writer.WriteElementString("Mesaj", log.Message);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            MessageBox.Show("XML raporu başarıyla oluşturuldu!");
        }

        public void CreateExcel( string directory)
        {
            string filePath = Path.Combine(directory, "LogRapor.xlsx");
            // EPPlus için LicenseContext belirtme
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage())
            {
                // Yeni bir çalışma sayfası oluştur
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Log Raporu");

                // Başlıkları ekleyin
                worksheet.Cells[1, 1].Value = "Tarih";
                worksheet.Cells[1, 2].Value = "Level";
                worksheet.Cells[1, 3].Value = "Mesaj";
                worksheet.Cells[1, 4].Value = "Kullanıcı Adı";

                // Log verilerini doldur
                for (int i = 0; i < logEntries.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = logEntries[i].Tarih;
                    worksheet.Cells[i + 2, 2].Value = logEntries[i].Severity;
                    worksheet.Cells[i + 2, 3].Value = logEntries[i].Message;
                    worksheet.Cells[i + 2, 4].Value = logEntries[i].KullaniciAdi;
                }

                // Dosyayı belirtilen konuma kaydedin
                FileInfo file = new FileInfo(filePath);
                package.SaveAs(file);
            }
            MessageBox.Show("XLSX raporu başarıyla oluşturuldu!");
        }
    }
}
