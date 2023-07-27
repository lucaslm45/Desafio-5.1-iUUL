using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternoTestes.Equipamento.Dtos
{
    public class IntegrarBicicletaDto
    {
        public Guid? IdTranca { get; set; }
        public Guid? IdBicicleta { get; set; }
        public int? IdFuncionario { get; set; }
    }
}
