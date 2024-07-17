using BookStore.Application.DTOs;
using BookStore.Application.Interfaces;
using BookStore.Infrastructure.Filters;
using BookStore.Infrastructure.Requests;
using BookStore.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.API.Controllers
{
    /// <summary>
    /// Controller for managing books.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogActionFilterAttribute<BookController>))]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookService bookService, ILogger<BookController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a paginated list of books based on the provided filter criteria.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/book/GetBooks?pageNumber=1&amp;pageSize=10&amp;name=Harry&amp;date=01.01.2023
        ///
        /// </remarks>
        /// <param name="request">The filter request containing optional name, date, pageNumber, and pageSize parameters.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        /// <returns>
        /// A paginated list of books that match the filter criteria.
        /// </returns>
        /// <response code="200">Returns the paginated list of books.</response>
        /// <response code="400">If the request is invalid or cannot be processed, returns an error message.</response>
        [AllowAnonymous]
        [HttpGet("GetBooks")]
        [ProducesResponseType(typeof(PagedResult<BookDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult<PagedResult<BookDto>>> GetAllBooks([FromQuery] BooksPagedFilterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var books = await _bookService.GetAllBooksAsync(request, cancellationToken);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a book by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the book to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        /// <returns>
        /// The book with the specified ID.
        /// </returns>
        /// <response code="200">Returns the requested book.</response>
        /// <response code="404">If a book with the specified ID does not exist.</response>
        /// <response code="400">If the request is invalid or cannot be processed, return an error message.</response>
        [AllowAnonymous]
        [HttpGet("GetBook/{id}")]
        [ProducesResponseType(typeof(BookDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult<BookDto>> GetBookById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id, cancellationToken);
                if (book == null)
                {
                    return NotFound();
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new book to the collection.
        /// </summary>
        /// <param name="bookCreateDto">The data transfer object containing book details.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        /// <returns>
        /// The created book.
        /// </returns>
        /// <response code="201">Returns the newly created book.</response>
        /// <response code="400">If the request is invalid or cannot be processed, return an error message.</response>
        [AllowAnonymous]
        [HttpPost("AddBook")]
        [ProducesResponseType(typeof(BookDto), 201)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult<BookDto>> AddBook(BookCreateDto bookCreateDto, CancellationToken cancellationToken)
        {
            try
            {
                var book = await _bookService.AddBookAsync(bookCreateDto, cancellationToken);
                return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing book in the collection.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="bookUpdateDto">The data transfer object containing updated book details.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        /// <returns>
        /// The updated book.
        /// </returns>
        /// <response code="200">Returns the updated book.</response>
        /// <response code="404">If a book with the specified ID does not exist.</response>
        /// <response code="400">If the request is invalid or cannot be processed, return an error message.</response>
        [AllowAnonymous]
        [HttpPut("UpdateBook/{id}")]
        [ProducesResponseType(typeof(BookDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BookDto>> UpdateBook([FromRoute]int id, BookUpdateDto bookUpdateDto, CancellationToken cancellationToken)
        {
            if (id != bookUpdateDto.Id)
            {
                _logger.LogWarning("Book ID mismatch: Requested ID {RequestedId} does not match DTO ID {DtoId}", id, bookUpdateDto.Id);
                return BadRequest("Book ID mismatch");
            }

            try
            {
                var updatedBook = await _bookService.UpdateBookAsync(bookUpdateDto, cancellationToken);
                if (updatedBook == null)
                {
                    return NotFound();
                }

                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a book from the collection by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        /// <returns>
        /// A boolean indicating whether the deletion was successful.
        /// </returns>
        /// <response code="200">Returns true if the book was successfully deleted.</response>
        /// <response code="404">If a book with the specified ID does not exist.</response>
        /// <response code="400">If the request is invalid or cannot be processed, return error message.</response>
        [AllowAnonymous]
        [HttpDelete("DeleteBook/{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult<bool>> DeleteBook(int id, CancellationToken cancellationToken)
        {
            try
            {
                var success = await _bookService.DeleteBookAsync(id, cancellationToken);
                if (!success)
                {
                    return NotFound();
                }

                return Ok(success);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
