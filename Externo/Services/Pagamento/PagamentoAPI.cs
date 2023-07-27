using Externo.Data.Dtos;
using Stripe;

namespace Externo.Services
{
    /// <summary>
    /// Gerencia o pagamento de cobranças usando a API Externa Stripe
    /// </summary>
    public class PagamentoAPI
    {
        /// <summary>
        /// Processa o pagamento usando a API externa de pagamentos STRIPE
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="cartao"></param>
        /// <returns></returns>
        public string ProcessaPagamento(decimal valor, ReadCartaoDto cartao)
        {
            // Cria um novo método de pagamento
            var paymentMethodService = new PaymentMethodService();
            var paymentMethod = paymentMethodService.Create(new PaymentMethodCreateOptions
            {
                Type = "card",
                Card = new PaymentMethodCardOptions
                {
                    Number = cartao.Numero,
                    ExpMonth = cartao.MesValidade,
                    ExpYear = cartao.AnoValidade,
                    Cvc = cartao.CodigoSeguranca,
                },
            });
            // Cria o objeto de opções do pagamento
            var options = new PaymentIntentCreateOptions
            {
                /*Todas as solicitações de API esperam que os valores sejam informados na menor unidade da moeda.
                 * Por exemplo, para cobrar USD 10, informe um amount de 1000 (1.000 centavos).*/
                Amount = (long)valor * 100,
                Currency = "BRL",
                PaymentMethod = paymentMethod.Id, // Usa o ID do método de pagamento criado acima
                Confirm = true,
            };

            // Cria o pagamento
            var service = new PaymentIntentService(StripeConfiguration.StripeClient);
            var paymentIntent = service.Create(options);

            return paymentIntent.Status;
        }
    }
}
