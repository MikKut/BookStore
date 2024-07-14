namespace BookStore.Application.DTOs
{
    public class BookCreateDto
    {
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Description { get; set; }
        public int NumberOfPages { get; set; }
    }
}
