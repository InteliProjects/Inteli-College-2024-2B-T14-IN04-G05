using WebApiBIMU.DTOs;

namespace WebApiBIMU.Helpers.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Eventos, AddEventoDto>().ReverseMap();
            CreateMap<Eventos, UpdateEventoDto>().ReverseMap();
            CreateMap<Eventos, GetEventoDto>().ReverseMap();
            //CreateMap<AddPessoaDto, Pessoas>().ReverseMap()
            //    .ForMember(dest => dest.PessoaFisica, act => act.MapFrom(src => src.PessoaFisica))
            //    .ForMember(dest => dest.PessoaJuridica, act => act.MapFrom(src => src.PessoaJuridica));
            //CreateMap<Pessoas, AddPessoaDto>().ReverseMap()
            //    .ForMember(dest => dest.PessoaFisica, act => act.MapFrom(src => src.PessoaFisica))
            //    .ForMember(dest => dest.PessoaJuridica, act => act.MapFrom(src => src.PessoaJuridica));
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //CreateMap<GetPessoasDto, Pessoas>()
            //    .ReverseMap()
            //    .ForMember(dest => dest.PessoaFisica, act => act.MapFrom(src => src.PessoaFisica))
            //    .ForMember(dest => dest.PessoaJuridica, act => act.MapFrom(src => src.PessoaJuridica));
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //CreateMap<UpdatePessoaDto, Pessoas>().ReverseMap()
            //    .ForMember(dest => dest.PessoaFisica, act => act.MapFrom(src => src.PessoaFisica))
            //    .ForMember(dest => dest.PessoaJuridica, act => act.MapFrom(src => src.PessoaJuridica));

            //CreateMap<Pessoas, UpdatePessoaDto>().ReverseMap()
            //    .ForMember(dest => dest.PessoaFisica, act => act.MapFrom(src => src.PessoaFisica))
            //    .ForMember(dest => dest.PessoaJuridica, act => act.MapFrom(src => src.PessoaJuridica));

        }
    }
}
