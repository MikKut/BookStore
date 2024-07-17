using BookStore.Domain.Entities;
using BookStore.Infrastructure.Requests;
using BookStore.Infrastructure.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Repositories
{
    public interface IBookRepository
    {
        Task<PagedResult<BookEntity>> GetAllBooksAsync(BooksPagedFilterRequest request, CancellationToken cancellationToken);
        Task<BookEntity> GetBookByIdAsync(int id, CancellationToken cancellationToken);
        Task<BookEntity> AddBookAsync(BookEntity book, CancellationToken cancellationToken);
        Task<BookEntity?> UpdateBookAsync(BookEntity book, CancellationToken cancellationToken);
        Task<bool> DeleteBookAsync(int id, CancellationToken cancellationToken);
        Task<IDictionary<int, int>> GetBooksPublicationDataAsync(CancellationToken cancellationToken);
    }
}
