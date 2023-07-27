using Externo.Data;
using Npgsql;
using System.Runtime.CompilerServices;

namespace Externo.Util
{
    /// <summary>
    /// Informações necessárias para conexão com banco e API's externas/internas
    /// </summary>
    public static class Conexao
    {
        private const string Host = "123.123.123";
        private const string Port = "123456";
        private const string Database = "gru";
        private const string Username = "gru";
        private const string Password = "123456";

        /// <summary>
        /// Texto necessário para conexão ao banco de dados da aplicação
        /// </summary>
        public const string ConnectionString = $"Host={Host};Port={Port};Database={Database};" +
            $"Username={Username};Password={Password}";

        /// <summary>
        /// Texto necessário para conexão ao banco de dados da aplicação Equipamento
        /// </summary>
        public const string ConnectionStringEquipamento = $"Host={Host};Port={Port};Database=grupo2_equipamento;" +
            $"Username=grupo2_usuario_equipamento;Password={Password}";

        /// <summary>
        /// Chave de acesso a API Externa Stripe usada para pagamentos
        /// </summary>
        public const string ConnectionStripe = "password";

        /// <summary>
        /// Chave de acesso a API Externa Send Grid usada para Emails
        /// </summary>
        public const string ConnectionSendGrid = "password";
        /// <summary>
        /// Chave de acesso a API Externa Truemail usada para validar até 50 Emails
        /// </summary>
        //public const string ConnectionURLTruemail = "https://truemail.io/api/v1/verify/single?address_info=1&timeout=100&access_token=";

        /// <summary>
        /// URL de conexão com API
        /// </summary>
        public const string ConnectionApiInterna = "https://residencia-nebula.ed.dev.br/externo-grupo2/";
        public const string ConnectionApiAluguel = "https://residencia-nebula.ed.dev.br/aluguel-grupo2/";
        public const string ConnectionApiEquipamento = "https://residencia-nebula.ed.dev.br/equipamento-grupo2/";
        //public const string ConnectionApiEquipamento = "https://localhost:7213/";
        //public const string ConnectionApiLocal = "https://localhost:7128/";


        public static void VerificaConexao(this AppDbContext context)
        {
            if (!context.Database.CanConnect())
                throw new NpgsqlException("Não foi possível conectar-se ao banco de dados.");
        }
    }
}
