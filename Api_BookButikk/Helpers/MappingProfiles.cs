using Api_BookButikk.Data;
using Api_BookButikk.Model;
using AutoMapper;

namespace Api_BookButikk.Helpers
{
    public class MappingProfiles:Profile
    {
        //needs to be dependancy injected into repository
        public MappingProfiles()
        {
            //create map in constructor
            CreateMap<Books, BookModel>().ReverseMap();
        }
        
    }
}
