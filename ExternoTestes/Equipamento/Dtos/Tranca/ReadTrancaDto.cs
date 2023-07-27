namespace ExternoTestes.Equipamento
{
    public class ReadTrancaDto
    {
        public Guid? Id { get; set; }
        public int? Numero { get; set; }
        public string? AnoDeFabricacao { get; set; }
        public string? Modelo { get; set; }
        public string? Localizacao { get; set; }
        public EStatusTranca? Status { get; set; }
        public string? Bicicleta { get; set; }
    }

}
