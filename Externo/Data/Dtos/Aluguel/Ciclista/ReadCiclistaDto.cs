namespace Externo.Data.Dtos
{
    public class Ciclista
    {
        public string id { get; set; }
        public string status { get; set; }
        public string nome { get; set; }
        public string nascimento { get; set; }
        public string cpf { get; set; }
        public Passaporte passaporte { get; set; }
        public string nacionalidade { get; set; }
        public string email { get; set; }
        public string urlFotoDocumento { get; set; }
    }

    public class Passaporte
    {
        public string numero { get; set; }
        public string validade { get; set; }
        public string pais { get; set; }
    }
}
