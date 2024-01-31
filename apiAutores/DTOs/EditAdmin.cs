using System.ComponentModel.DataAnnotations;

namespace apiAutores.DTOs
{
    public class EditAdmin
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
