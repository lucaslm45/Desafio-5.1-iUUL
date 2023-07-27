using Stripe;

namespace Externo.Util
{
    public static class Extensions
    {
        /// <summary>
        /// Retorna o conteúdo retornado pelo endpoint e o tipo de resposta
        /// </summary>
        /// <param name="_endpoint"></param>
        /// <returns></returns>
        public static (string, HttpResponseMessage) GetInformacaoExterna(string _endpoint)
        {
            var client = new HttpClient();
            var response = client.GetAsync(_endpoint).Result;

            var content = response.Content.ReadAsStringAsync().Result;

            return (content, response);
        }
        /// <summary>
        /// Envia informação para um endpoint Externo
        /// </summary>
        /// <param name="_endpoint"></param>
        /// <param name="_httpContent"></param>
        /// <returns></returns>
        public static (string, HttpResponseMessage) PostInformacao(string _endpoint, HttpContent _httpContent)
        {
            var client = new HttpClient();
            var response = client.PostAsync(_endpoint, _httpContent).Result;

            var content = response.Content.ReadAsStringAsync().Result;

            return (content, response);
        }
    }
}
