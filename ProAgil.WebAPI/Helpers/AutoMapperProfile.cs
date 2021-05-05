using AutoMapper;
using ProAgil.Domain;
using ProAgil.WebAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProAgil.WebAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Evento, EventoDto>() // estabelece relação de muitos para muitos
                .ForMember(d => d.Palestrantes, opt => {
                    opt.MapFrom(src => src.PalestrantesEventos.Select(p => p.Palestrante).ToList());
                });

            CreateMap<Palestrante, PalestranteDto>() // estabelece relação de muitos para muitos
                .ForMember(d => d.Eventos, opt => {
                    opt.MapFrom(src => src.PalestrantesEventos.Select(e => e.Evento).ToList());
                });

            CreateMap<Lote, LoteDto>();

            CreateMap<RedeSocial, RedeSocialDto>();
        }
    }
}
