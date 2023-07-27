using Externo.Models;
using Externo.Util;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Externo.Data.Dtos
{
    [SwaggerSchema(Title = SchemaNames.Cobranca)]
    public class ReadCobrancaDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; }

        private DateTime horaSolicitacao;

        public DateTime HoraSolicitacao
        {
            get { return horaSolicitacao; }
            set { horaSolicitacao = value.ToLocalTime(); }
        }
        private DateTime horaFinalizacao;
        public DateTime HoraFinalizacao
        {
            get { return horaFinalizacao; }
            set { horaFinalizacao = value.ToLocalTime(); }
        }

        public decimal Valor { get; set; }
        public Guid Ciclista { get; set; }
    }
}
