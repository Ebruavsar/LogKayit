using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG.BusinessLogic
{
    public class LogEntry
    {
        public DateTime Tarih { get; set; }
        public string Severity { get; set; }
        public string Message { get; set; }
        public string KullaniciAdi { get; set; }
    }
}
