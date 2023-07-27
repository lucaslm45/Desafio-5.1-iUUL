using Externo.Util;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Externo.Data.Dtos
{
    [SwaggerSchema(Title = SchemaNames.Erro)]

    public class ReadErroDto
    {
        [Required]
        public string Codigo { get; set; }
        [Required]
        public string Mensagem { get; set; }
    }
}
