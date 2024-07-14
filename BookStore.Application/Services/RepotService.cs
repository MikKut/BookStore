using BookStore.Application.Interfaces;
using BookStore.Infrastructure.Repositories;

namespace BookStore.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IBookRepository _bookRepository;

        public ReportService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IDictionary<int, int>> GetBooksPublicationDataAsync(CancellationToken cancellationToken)
        {
            return await _bookRepository.GetBooksPublicationDataAsync(cancellationToken);
        }
    }
}
