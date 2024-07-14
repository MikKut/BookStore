using AutoMapper;
using BookStore.Application.DTOs;
using BookStore.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookStore.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookEntity, BookDto>().ReverseMap();
            CreateMap<BookCreateDto, BookEntity>().ReverseMap();
            CreateMap<BookUpdateDto, BookEntity>().ReverseMap();
        }
    }
}
