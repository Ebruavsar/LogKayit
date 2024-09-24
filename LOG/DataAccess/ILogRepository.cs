using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LOG.DataAccess.FileLogRepository;

namespace LOG.DataAccess
{
    public interface ILogRepository
    {
        void WriteLog(LogLevel level, string message, string userName); // Log dosyasına yazma işlemi
    }
}
