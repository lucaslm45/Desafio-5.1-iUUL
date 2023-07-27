using Externo.Data.Dtos;
using Externo.Util;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Net.Mail;

namespace Externo.Services
{
    public class EmailAPI
    {
        public async Task<Response> EnviaEmail(CreateEmailDto requisicao)
        {
            /// Valida email antes de tentar enviar
            /// Se não valido: 
            ///     throw new NotFoundException();
            var client = new SendGridClient(Conexao.ConnectionSendGrid);

            // Cria uma nova mensagem
            var message = new SendGridMessage();

            // Define o remetente e o destinatário
            var from = new MailAddress(EmailGenerico.RemetenteEmail, EmailGenerico.RemetenteNome);
            var to = new MailAddress(requisicao.Email);
            message.SetFrom(from.Address, from.DisplayName);
            message.AddTo(to.Address);

            // Define o assunto da mensagem
            message.SetSubject(requisicao.Assunto);

            // Adiciona o conteúdo do e-mail
            message.AddContent(MimeType.Text, requisicao.Mensagem);

            return await client.SendEmailAsync(message);
        }
    }
}
