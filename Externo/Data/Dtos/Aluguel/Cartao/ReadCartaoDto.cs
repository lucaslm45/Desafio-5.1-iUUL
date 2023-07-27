namespace Externo.Data.Dtos
{
    public class ReadCartaoDto
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Numero { get; set; }
        public long MesValidade { get; set; }
        public long AnoValidade { get; set; }
        public string CodigoSeguranca { get; set; }
    }
}



