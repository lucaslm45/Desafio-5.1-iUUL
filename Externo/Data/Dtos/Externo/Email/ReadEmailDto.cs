using Externo.Util;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Externo.Data.Dtos
{
    [SwaggerSchema(Title = SchemaNames.Email)]

    public class ReadEmailDto
    {
        public Guid Id { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Assunto { get; set; }
        public string Mensagem { get; set; }

    }
}
