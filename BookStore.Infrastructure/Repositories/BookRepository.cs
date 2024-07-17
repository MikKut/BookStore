using AutoMapper;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Data;
using BookStore.Infrastructure.Requests;
using BookStore.Infrastructure.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BookRepository> _logger;
        private readonly IMapper _mapper;
        public BookRepository(ApplicationDbContext context, ILogger<BookRepository> logger, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public async Task<PagedResult<BookEntity>> GetAllBooksAsync(BooksPagedFilterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.Books.AsQueryable();

                if (!string.IsNullOrEmpty(request.Name))
                {
                    query = query.Where(b => b.Title.Contains(request.Name));
                }

                if (!string.IsNullOrEmpty(request.StartDate))
                {
                    if (DateTime.TryParse(request.StartDate,  out var date))
                    {
                        query = query.Where(b => b.PublicationDate >= date);
                    }
                }

                if (!string.IsNullOrEmpty(request.EndDate))
                {
                    if (DateTime.TryParse(request.EndDate,  out var date))
                    {
                        query = query.Where(b => b.PublicationDate <= date);
                    }
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var books = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);


                return new PagedResult<BookEntity> {Items = books, TotalCount = totalCount };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllBooksAsync");
                throw;
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
                var existingBook = await _context.Books.FindAsync(new object[] { book.Id }, cancellationToken);
                if (existingBook == null)
                {
                    return null;
                }

                existingBook.Title = book.Title;
                existingBook.PublicationDate = book.PublicationDate;
                existingBook.Description = book.Description;
                existingBook.NumberOfPages = book.NumberOfPages;

                await _context.SaveChangesAsync(cancellationToken);
                return existingBook;
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
