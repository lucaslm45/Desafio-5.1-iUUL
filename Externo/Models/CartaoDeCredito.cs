namespace Externo.Models
{
    public class CartaoDeCredito
    {
        public Guid Id { get; set; }
        public string NomeTitular { get; set; }
        public string Numero { get; set; }
        public string Validade { get; set; }
        public string Cvv { get; set; }
    }
}
