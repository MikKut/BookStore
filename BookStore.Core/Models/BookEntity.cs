namespace BookStore.Core.Models
{
    public class BookEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Description { get; set; }
        public int NumberOfPages { get; set; }
    }
}
