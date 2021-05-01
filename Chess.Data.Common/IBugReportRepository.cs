using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Data.Common.Models;

namespace Chess.Data.Common
{
    public interface IBugReportRepository
    {
        Task<IEnumerable<BugReport>> GetReports(int skip, int limit);
    }
}
