using AutoMapper;
using AbpResearchWorker.Books;

namespace AbpResearchWorker;

public class AbpResearchWorkerApplicationAutoMapperProfile : Profile
{
    public AbpResearchWorkerApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
