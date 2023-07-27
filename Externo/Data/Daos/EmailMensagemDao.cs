using Externo.Models;
using Externo.Util;

namespace Externo.Data.Daos
{
    public class EmailMensagemDao
    {
        private readonly AppDbContext context;

        public EmailMensagemDao(AppDbContext _context)
        {
            context = _context;
        }

        public EmailMensagem Add(string _assunto, string _email, string _mensagem)
        {
            context.VerificaConexao();

            var email = new EmailMensagem
            {
                EmailAddress = _email,
                Assunto = _assunto,
                Mensagem = _mensagem,
            };

            context.Emails.Add(email);
            context.SaveChanges();

            return email;
        }
    }
}
