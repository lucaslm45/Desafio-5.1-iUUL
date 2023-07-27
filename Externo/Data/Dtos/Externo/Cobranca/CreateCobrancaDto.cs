using Externo.Util;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Externo.Data.Dtos
{
    [SwaggerSchema(Title = SchemaNames.NovaCobranca)]
    public class CreateCobrancaDto
    {
        [ModelBinder(Name = Erro.ValorInvalidoCod)]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = Erro.ValorInvalidoMsg)]
        [Required]
        public decimal Valor { get; set; }
        [Required]
        public Guid Ciclista { get; set; }
    }
}
