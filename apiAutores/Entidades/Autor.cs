﻿using apiAutores.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apiAutores.Entidades
{
    public class Autor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres")]
        [FirstCharUpper]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "No es un email valido")]
        [NotMapped]
        public string Email { get; set; }
        public List<Libro>? Libros { get; set; }

    }
}
