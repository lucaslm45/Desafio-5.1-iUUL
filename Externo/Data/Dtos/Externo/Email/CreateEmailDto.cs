using Externo.Util;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Externo.Data.Dtos
{
    [SwaggerSchema(Title = SchemaNames.NovoEmail)]
    public class CreateEmailDto
    {
        [Required]
        [ModelBinder(Name = Erro.NotEmailAddressCod)]
        [DataType(DataType.EmailAddress, ErrorMessage = Erro.NotEmailAddressMsg)]
        public string Email { get; set; }
        [Required]
        public string Assunto { get; set; }
        [Required]
        public string Mensagem { get; set; }
    }
}
