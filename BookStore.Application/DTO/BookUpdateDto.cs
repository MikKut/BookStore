namespace BookStore.Application.DTOs
{
    public class BookUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Description { get; set; }
        public int NumberOfPages { get; set; }
    }
}
