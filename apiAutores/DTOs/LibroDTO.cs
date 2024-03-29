﻿using apiAutores.Validations;
using System.ComponentModel.DataAnnotations;

namespace apiAutores.DTOs
{
    public class LibroDTO
    {
        [Required]
        [StringLength(120)]
        [FirstCharUpper]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public List<int> AutoresIds { get; set; }
    }
}
 