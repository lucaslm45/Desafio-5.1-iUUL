using System.ComponentModel.DataAnnotations;

namespace Externo.Models
{
    public class Cobranca
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid Ciclista { get; set; }
        [Required]
        public decimal Valor { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime HoraSolicitacao { get; set; }
        [Required]
        public DateTime HoraFinalizacao { get; set; }
    }
}
