using BookStore.Application.DTOs;
using BookStore.Infrastructure.Requests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Application.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync(BooksPagedFilterRequest request, CancellationToken cancellationToken);
        Task<BookDto> GetBookByIdAsync(int id, CancellationToken cancellationToken);
        Task<BookDto> AddBookAsync(BookCreateDto bookCreateDto, CancellationToken cancellationToken);
        Task<BookDto?> UpdateBookAsync(BookUpdateDto bookUpdateDto, CancellationToken cancellationToken);
        Task<bool> DeleteBookAsync(int id, CancellationToken cancellationToken);
    }
}
