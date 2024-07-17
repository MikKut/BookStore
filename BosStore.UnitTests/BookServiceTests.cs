using Moq;
using AutoMapper;
using BookStore.Application.Services;
using BookStore.Domain.Entities;
using BookStore.Application.DTOs;
using BookStore.Infrastructure.Repositories;
using BookStore.Infrastructure.Responses;
using BookStore.Infrastructure.Requests;

namespace BosStore.UnitTests
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly IMapper _mapper;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookEntity, BookDto>();
                cfg.CreateMap<BookCreateDto, BookEntity>();
                cfg.CreateMap<BookUpdateDto, BookEntity>();
                cfg.CreateMap<PagedResult<BookEntity>, PagedResult<BookDto>>();
            });
            _mapper = config.CreateMapper();
            _bookService = new BookService(_bookRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllBooksAsync_ShouldReturnPagedResult()
        {
            // Arrange
            var books = new List<BookEntity>
        {
            new BookEntity { Id = 1, Title = "Test Book 1" },
            new BookEntity { Id = 2, Title = "Test Book 2" }
        };
            var pagedResult = new PagedResult<BookEntity>
            {
                Items = books,
                TotalCount = 2
            };
            var request = new BooksPagedFilterRequest { PageNumber = 1, PageSize = 10 };

            _bookRepositoryMock.Setup(repo => repo.GetAllBooksAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _bookService.GetAllBooksAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.Items.Count());
            _bookRepositoryMock.Verify(repo => repo.GetAllBooksAsync(request, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldReturnBookDto()
        {
            // Arrange
            var book = new BookEntity { Id = 1, Title = "Test Book" };

            _bookRepositoryMock.Setup(repo => repo.GetBookByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            // Act
            var result = await _bookService.GetBookByIdAsync(1, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Book", result.Title);
            _bookRepositoryMock.Verify(repo => repo.GetBookByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddBookAsync_ShouldReturnAddedBookDto()
        {
            // Arrange
            var bookCreateDto = new BookCreateDto { Title = "New Book" };
            var bookEntity = new BookEntity { Id = 1, Title = "New Book" };

            _bookRepositoryMock.Setup(repo => repo.AddBookAsync(It.IsAny<BookEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookEntity);

            // Act
            var result = await _bookService.AddBookAsync(bookCreateDto, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("New Book", result.Title);
            _bookRepositoryMock.Verify(repo => repo.AddBookAsync(It.IsAny<BookEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateBookAsync_ShouldReturnUpdatedBookDto()
        {
            // Arrange
            var bookUpdateDto = new BookUpdateDto { Id = 1, Title = "Updated Book" };
            var bookEntity = new BookEntity { Id = 1, Title = "Updated Book" };

            _bookRepositoryMock.Setup(repo => repo.UpdateBookAsync(It.IsAny<BookEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookEntity);

            // Act
            var result = await _bookService.UpdateBookAsync(bookUpdateDto, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Updated Book", result.Title);
            _bookRepositoryMock.Verify(repo => repo.UpdateBookAsync(It.IsAny<BookEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldReturnTrue()
        {
            // Arrange
            _bookRepositoryMock.Setup(repo => repo.DeleteBookAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _bookService.DeleteBookAsync(1, CancellationToken.None);

            // Assert
            Assert.True(result);
            _bookRepositoryMock.Verify(repo => repo.DeleteBookAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }
    }

}