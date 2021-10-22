using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.Domain
{
    public class Evento
    {
        [Key]
        public int Id { get; set; }

        public string Local { get; set; }

        public DateTime DataEvento { get; set; }

        [Required(ErrorMessage = "O tema deve ser obrigátorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O tema deve ser entre 3 e 100 caracteres")]
        public string Tema { get; set; }

        [Range(2, 1000, ErrorMessage = "A quantidade de pessoas devem ser entre 2 e 1000")]
        public int QtdPessoas { get; set; }

        public string ImagemUrl { get; set; }

        [Phone]
        public string Telefone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public List<Lote> Lotes { get; set; }

        public List<RedeSocial> RedesSociais { get; set; }

        public List<PalestranteEvento> PalestrantesEventos { get; set; }

    }
}