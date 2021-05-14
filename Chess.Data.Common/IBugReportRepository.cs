using Chess.Data.Common.Models.V1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chess.Data.Common
{
    public interface IBugReportRepository
    {
        Task AddReport(BugReportDto report);
        Task<IEnumerable<BugReportDto>> GetReports(int skip, int limit);
    }
}
