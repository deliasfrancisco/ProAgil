﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProAgil.WebAPI.Dtos
{
    public class RedeSocialDto
    {
		public int Id { get; set; }
		public string Nome { get; set; }
        public string URL { get; set; }
        public EventoDto Evento { get; set; }
        public PalestranteDto Palestrante { get; }
    }
}
