using AutoMapper;
using Entities.DataTransferObject;
using Entities.Models;

namespace WepApi.Utilities.AutoMapper
{
    //automapperprofile sınıfı olmuş oldu 
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //reverse işlemi iki yönlğ çalışmaya yarar 
            CreateMap<BookDtoForUpdate, Book>().ReverseMap();
            CreateMap<Book, BookDto>();
            CreateMap<BookDtoForInsertion, Book>();
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}
