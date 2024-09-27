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
using System.Xml.Linq;
using System.Windows.Media;
using PdfSharp.Pdf;
using PdfSharp.Drawing;


namespace LOG
{
    public partial class Raporla : Form
    {
        private LogService logService;
        private List<LogEntry> logEntries;

        public Raporla(DataTable logTable)
        {
            InitializeComponent();
            logService = new LogService();

            // DataTable'dan logEntries listesine log verilerini aktarma
            logEntries = logTable.AsEnumerable().Select(row => new LogEntry
            {
                Tarih = row.Field<DateTime>("Tarih"),
                Severity = row.Field<string>("Level"),
                Message = row.Field<string>("Mesaj"),
                KullaniciAdi = row.Field<string>("Kullanıcı Adı")
            }).ToList();

            // Format seçenekleri ekleme
            comboBoxFormat.Items.AddRange(new string[] { "PDF", "XML", "XLSX" });
            comboBoxFormat.SelectedIndex = 0;

            // Buton olaylarını bağlama
            buttonBrowse.Click += ButtonBrowse_Click;
            buttonRaporla.Click += ButtonRaporla_Click;
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    label1.Text = dialog.SelectedPath;
                }
            }
        }

        private void ButtonRaporla_Click(object sender, EventArgs e)
        {
            string selectedFormat = comboBoxFormat.SelectedItem.ToString();
            string directory = label1.Text;

            if (string.IsNullOrEmpty(directory))
            {
                MessageBox.Show("Lütfen bir dizin seçin.");
                return;
            }

            try
            {
                switch (selectedFormat)
                {
                    case "PDF":
                        ExportToPdf(directory);
                        break;
                    case "XML":
                        ExportToXml(directory);
                        break;
                    case "XLSX":
                        CreateExcel(directory);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Rapor oluşturulurken bir hata oluştu: {ex.Message}");
            }
        }

        private void ExportToPdf(string directory)
        {
            string filePath = Path.Combine(directory, "LogRapor.pdf");

            try
            {
                // PdfSharp ile yeni PDF belgesi oluştur
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Log Raporu";

                // Yeni sayfa ekle
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Verdana", 12);

                // Başlık ekle
                gfx.DrawString("Log Raporu", new XFont("Verdana", 14), XBrushes.Black,
                    new XRect(0, 0, page.Width, 50), XStringFormats.TopCenter);
                gfx.DrawString($"Rapor Tarihi: {DateTime.Now}", font, XBrushes.Black, 50, 50);

                // Tablonun başlıklarını ekle ve renkli hücreler yap
                int xTarih = 50, xLevel = 200, xMesaj = 250, xKullaniciAdi = 450;
                int y = 100; // Başlangıç y koordinatı
                int cellHeight = 20; // Hücre yüksekliği
                int cellWidthTarih = 150, cellWidthLevel = 80, cellWidthMesaj = 200, cellWidthKullaniciAdi = 100; // Hücre genişlikleri

                // Tablo başlıkları için hücreler
                gfx.DrawRectangle(XBrushes.DarkGreen, xTarih, y, cellWidthTarih, cellHeight);
                gfx.DrawRectangle(XBrushes.DarkGreen, xLevel, y, cellWidthLevel, cellHeight);
                gfx.DrawRectangle(XBrushes.DarkGreen, xMesaj, y, cellWidthMesaj, cellHeight);
                gfx.DrawRectangle(XBrushes.DarkGreen, xKullaniciAdi, y, cellWidthKullaniciAdi, cellHeight);

                gfx.DrawString("Tarih", font, XBrushes.BlanchedAlmond, new XRect(xTarih, y, cellWidthTarih, cellHeight), XStringFormats.Center);
                gfx.DrawString("Level", font, XBrushes.BlanchedAlmond, new XRect(xLevel, y, cellWidthLevel, cellHeight), XStringFormats.Center);
                gfx.DrawString("Mesaj", font, XBrushes.BlanchedAlmond, new XRect(xMesaj, y, cellWidthMesaj, cellHeight), XStringFormats.Center);
                gfx.DrawString("Kullanıcı Adı", font, XBrushes.BlanchedAlmond, new XRect(xKullaniciAdi, y, cellWidthKullaniciAdi, cellHeight), XStringFormats.Center);

                // Log verilerini eklemek için yPoint ayarı
                y += cellHeight;

                // Log verilerini PDF'e ekle (tablo satırları)
                foreach (var log in logEntries)
                {
                    // Hücreleri renklendir
                    gfx.DrawRectangle(XBrushes.LightGray, xTarih, y, cellWidthTarih, cellHeight);
                    gfx.DrawRectangle(XBrushes.LightGray, xLevel, y, cellWidthLevel, cellHeight);
                    gfx.DrawRectangle(XBrushes.LightGray, xMesaj, y, cellWidthMesaj, cellHeight);
                    gfx.DrawRectangle(XBrushes.LightGray, xKullaniciAdi, y, cellWidthKullaniciAdi, cellHeight);

                    // Hücre içerikleri
                    gfx.DrawString(log.Tarih.ToString(), font, XBrushes.Black, new XRect(xTarih, y, cellWidthTarih, cellHeight), XStringFormats.Center);
                    gfx.DrawString(log.Severity, font, XBrushes.Black, new XRect(xLevel, y, cellWidthLevel, cellHeight), XStringFormats.Center);
                    gfx.DrawString(log.Message, font, XBrushes.Black, new XRect(xMesaj, y, cellWidthMesaj, cellHeight), XStringFormats.Center);
                    gfx.DrawString(log.KullaniciAdi, font, XBrushes.Black, new XRect(xKullaniciAdi, y, cellWidthKullaniciAdi, cellHeight), XStringFormats.Center);

                    y += cellHeight; // Bir sonraki satırın y konumunu ayarlayın
                }

                // PDF dosyasını kaydet
                document.Save(filePath);
                document.Close();

                MessageBox.Show("PDF raporu başarıyla oluşturuldu!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PDF oluşturulurken bir hata oluştu: {ex.Message}");
            }

        }

        private void ExportToXml(string directory)
        {
            string filePath = Path.Combine(directory, "LogRapor.xml");
            try
            {
                using (XmlWriter writer = XmlWriter.Create(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Logs");

                    foreach (var log in logEntries)
                    {
                        writer.WriteStartElement("Log");
                        writer.WriteElementString("Tarih", log.Tarih.ToString());
                        writer.WriteElementString("Level", log.Severity);
                        writer.WriteElementString("Mesaj", log.Message);
                        writer.WriteElementString("KullanıcıAdı", log.KullaniciAdi);
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

                MessageBox.Show("XML raporu başarıyla oluşturuldu!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"XML raporu oluşturulurken hata: {ex.Message}");
            }
        }

        public void CreateExcel(string directory)
        {
            string filePath = Path.Combine(directory, "LogRapor.xlsx");
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Log Raporu");
                    worksheet.Cells[1, 1].Value = "Tarih";
                    worksheet.Cells[1, 2].Value = "Level";
                    worksheet.Cells[1, 3].Value = "Mesaj";
                    worksheet.Cells[1, 4].Value = "Kullanıcı Adı";

                    for (int i = 0; i < logEntries.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = logEntries[i].Tarih;
                        worksheet.Cells[i + 2, 2].Value = logEntries[i].Severity;
                        worksheet.Cells[i + 2, 3].Value = logEntries[i].Message;
                        worksheet.Cells[i + 2, 4].Value = logEntries[i].KullaniciAdi;
                    }

                    FileInfo file = new FileInfo(filePath);
                    package.SaveAs(file);
                }

                MessageBox.Show("XLSX raporu başarıyla oluşturuldu!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"XLSX raporu oluşturulurken hata: {ex.Message}");
            }
        }
    }
}
