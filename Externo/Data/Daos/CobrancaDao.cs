using Externo.Models;
using Externo.Util;

namespace Externo.Data.Daos
{
    public class CobrancaDao
    {
        private readonly AppDbContext context;

        public CobrancaDao(AppDbContext _context)
        {
            context = _context;
        }

        public Cobranca? GetById(Guid id)
        {
            context.VerificaConexao();
            return context.Cobrancas.FirstOrDefault(cobranca => cobranca.Id == id);
        }
        public IList<Cobranca> GetFilaPendente()
        {
            context.VerificaConexao();

            return context.Cobrancas
                          .Where(c => c.Status == EStatusCobranca.Pendente)
                          .OrderBy(c => c.Id).ToList();
        }
        public Cobranca Add(Guid _ciclista, decimal _valor, string _status, DateTime _solicitacao)
        {
            context.VerificaConexao();

            var cobranca = new Cobranca
            {
                Ciclista = _ciclista,
                Valor = _valor,
                Status = _status,
                HoraSolicitacao = _solicitacao
            };

            context.Cobrancas.Add(cobranca);
            context.SaveChanges();
            cobranca.HoraFinalizacao = DateTime.UtcNow;
            Update(cobranca);

            return cobranca;
        }
        public void Update(Cobranca _cobranca)
        {
            context.VerificaConexao();
            context.Cobrancas.Update(_cobranca);
            context.SaveChanges();
        }
    }
}
