using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternoTestes.Equipamento
{
    public enum EStatusTranca
    {
        NOVA,
        LIVRE,
        OCUPADA,
        REPARO_SOLICITADO,
        EM_REPARO,
        APOSENTADA,
        EXCLUIDA
    }
}
