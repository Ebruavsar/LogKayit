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
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Exceptions;

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

            logEntries = logTable.AsEnumerable().Select(row => new LogEntry
            {
                Tarih = row.Field<DateTime>("Tarih"),
                Severity = row.Field<string>("Level"),
                Message = row.Field<string>("Mesaj"),
                KullaniciAdi = row.Field<string>("Kullanıcı Adı")
            }).ToList();

            comboBoxFormat.Items.AddRange(new string[] { "PDF", "XML", "XLSX" });
            comboBoxFormat.SelectedIndex = 0;

            buttonBrowse.Click += ButtonBrowse_Click;
            buttonRaporla.Click += ButtonRaporla_Click;
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxDizin.Text = dialog.SelectedPath;
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
                using (PdfWriter writer = new PdfWriter(filePath))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        Document document = new Document(pdf);

                        // Başlık
                        document.Add(new Paragraph("Log Raporu"));
                        document.Add(new Paragraph($"Rapor Tarihi: {DateTime.Now}"));
                        document.Add(new Paragraph("\n"));

                        // Tablo oluştur
                        Table table = new Table(4); // 4 sütun
                        table.SetWidth(UnitValue.CreatePercentValue(100));

                        // Başlıklar
                        table.AddHeaderCell("Tarih");
                        table.AddHeaderCell("Seviye");
                        table.AddHeaderCell("Mesaj");
                        table.AddHeaderCell("Kullanıcı Adı");

                        // Log verilerini ekle
                        foreach (var log in logEntries)
                        {
                            table.AddCell(log.Tarih.ToString());
                            table.AddCell(log.Severity);
                            table.AddCell(log.Message);
                            table.AddCell(log.KullaniciAdi);
                        }

                        document.Add(table);
                        document.Close();
                    }
                }

                MessageBox.Show("PDF raporu başarıyla oluşturuldu!");
            }
            catch (PdfException pdfEx)
            {
                MessageBox.Show($"PDF hatası: {pdfEx.Message}\n{pdfEx.StackTrace}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}\n{ex.StackTrace}");
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
