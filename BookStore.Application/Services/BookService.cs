using AutoMapper;
using BookStore.Application.DTOs;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Repositories;
using BookStore.Infrastructure.Requests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync(BooksPagedFilterRequest request, CancellationToken cancellationToken)
        {
            var books = await _bookRepository.GetAllBooksAsync(request, cancellationToken);
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> GetBookByIdAsync(int id, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookByIdAsync(id, cancellationToken);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> AddBookAsync(BookCreateDto bookCreateDto, CancellationToken cancellationToken)
        {
            try
            {
                var book = _mapper.Map<BookEntity>(bookCreateDto);
                var addedBook = await _bookRepository.AddBookAsync(book, cancellationToken);
                return _mapper.Map<BookDto>(addedBook);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<BookDto?> UpdateBookAsync(BookUpdateDto bookUpdateDto, CancellationToken cancellationToken)
        {
            var book = _mapper.Map<BookEntity>(bookUpdateDto);
            var updatedBook = await _bookRepository.UpdateBookAsync(book, cancellationToken);
            return _mapper.Map<BookDto>(updatedBook);
        }

        public async Task<bool> DeleteBookAsync(int id, CancellationToken cancellationToken)
        {
            return await _bookRepository.DeleteBookAsync(id, cancellationToken);
        }
    }
}
