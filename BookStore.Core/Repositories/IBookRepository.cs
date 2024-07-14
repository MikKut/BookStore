using BookStore.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Core.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookEntity>> GetAllBooksAsync();
        Task<BookEntity> GetBookByIdAsync(int id);
        Task AddBookAsync(BookEntity book);
        Task UpdateBookAsync(BookEntity book);
        Task DeleteBookAsync(int id);
    }
}
