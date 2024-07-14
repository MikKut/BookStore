using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Application.Interfaces
{
    public interface IReportService
    {
        Task<IDictionary<int, int>> GetBooksPublicationDataAsync(CancellationToken cancellationToken);
    }
}
