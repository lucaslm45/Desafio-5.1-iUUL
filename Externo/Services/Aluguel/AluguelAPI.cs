using Externo.Util;

namespace Externo.Services
{
    public class AluguelAPI
    {
        private readonly HttpClient client;

        public AluguelAPI()
        {
            client = new();
        }
        /// <summary>
        /// Retorna o cartão de crédito de um ciclista
        /// </summary>
        /// <param name="ciclistaId"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetCartaoCiclista(Guid ciclistaId)
        {
            return await client.GetAsync(Conexao.ConnectionApiAluguel +
                                         Endpoints.CartaoDeCredito +
                                         ciclistaId);
        }
        /// <summary>
        /// Retorna o email de um ciclista
        /// </summary>
        /// <param name="ciclistaId"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetCiclista(Guid ciclistaId)
        {
            return await client.GetAsync(Conexao.ConnectionApiAluguel +
                                                 Endpoints.Ciclista +
                                                 ciclistaId);
        }
    }
}
