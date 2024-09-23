using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG.DataAccess
{
    public interface ILogRepository
    {
        void WriteLog(string message); // Log dosyasına yazma işlemi
    }
}
