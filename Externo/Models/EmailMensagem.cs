using System.ComponentModel.DataAnnotations;

namespace Externo.Models
{
    public class EmailMensagem
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Assunto { get; set; }
        [Required]
        public string Mensagem { get; set; }
    }
}
