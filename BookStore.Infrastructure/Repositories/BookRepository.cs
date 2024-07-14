using BookStore.Domain.Entities;
using BookStore.Infrastructure.Data;
using BookStore.Infrastructure.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(ApplicationDbContext context, ILogger<BookRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<BookEntity>> GetAllBooksAsync(BooksPagedFilterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.Books.AsQueryable();

                if (!string.IsNullOrEmpty(request.Name))
                {
                    query = query.Where(b => b.Title.Contains(request.Name));
                }

                if (!string.IsNullOrEmpty(request.Date))
                {
                    if (DateTime.TryParse(request.Date, out var date))
                    {
                        query = query.Where(b => b.PublicationDate == date);
                    }
                }

                return await query.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllBooksAsync");
                throw; // Re-throw the exception to propagate it to the caller
            }
        }

        public async Task<BookEntity> GetBookByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Books.FindAsync([id], cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBookByIdAsync");
                throw;
            }
        }

        public async Task<BookEntity> AddBookAsync(BookEntity book, CancellationToken cancellationToken)
        {
            try
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync(cancellationToken);
                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddBookAsync");
                throw;
            }
        }

        public async Task<BookEntity?> UpdateBookAsync(BookEntity book, CancellationToken cancellationToken)
        {
            try
            {
                var innerBook = await GetBookByIdAsync(book.Id, cancellationToken);
                if (innerBook == null)
                {
                    return null;
                }

                _context.Entry(book).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateBookAsync");
                throw;
            }
        }

        public async Task<bool> DeleteBookAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var book = await _context.Books.FindAsync(new object[] { id }, cancellationToken);
                if (book == null)
                {
                    return false;
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteBookAsync");
                throw;
            }
        }

        public async Task<IDictionary<int, int>> GetBooksPublicationDataAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Books
                    .GroupBy(b => b.PublicationDate.Year)
                    .Select(g => new { Year = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Year, x => x.Count, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBooksPublicationDataAsync");
                throw;
            }
        }
    }
}
