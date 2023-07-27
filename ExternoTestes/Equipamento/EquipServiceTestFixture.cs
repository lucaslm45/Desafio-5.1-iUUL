using Externo.Util;
using ExternoTestes.Equipamento.Dtos;
using Moq;
using Npgsql;
using System.Net.Http.Json;
using System.Security.Policy;

namespace ExternoTestes.Equipamento
{
    /// <summary>
    /// Classe para criação de dados no módulo Equipamento para atender o caso de uso 03
    /// </summary>
    public class EquipServiceTestFixture : IDisposable
    {
        private IList<ReadTrancaDto> trancas;
        private IList<ReadBicicletaDto> bicicletas;
        private ReadTotemDto totem;
        private readonly string urlTranca = Conexao.ConnectionApiEquipamento + Endpoints.Tranca;
        private readonly string urlBike = Conexao.ConnectionApiEquipamento + Endpoints.Bicicleta;
        private readonly string urlTotem = Conexao.ConnectionApiEquipamento + Endpoints.Totem;

        public NpgsqlConnection Db;

        public EquipServiceTestFixture()
        {
            Db = new NpgsqlConnection(Conexao.ConnectionStringEquipamento);

            trancas = Extensions.CriaTrancasAsync<ReadTrancaDto>(urlTranca).Result;
            bicicletas = Extensions.CriaBicicletasAsync<ReadBicicletaDto>(urlBike).Result;
            totem = Extensions.CriaTotemAsync<ReadTotemDto>(urlTotem).Result;

            //Integrar Trancas e Bicicletas na Rede
            var i = 0;
            foreach (var tranca in trancas)
            {
                var obj = new IntegrarTrancaDto
                {
                    IdTotem = totem.Id,
                    IdTranca = tranca.Id,
                    IdFuncionario = It.IsAny<int>()
                };

                _ = Extensions.SolicitaRequisicaoAsync<ReadTrancaDto>(urlTranca + Endpoints.Integrar,
                    HttpMethod.Post, null, null, obj);

                using (var client = new HttpClient())
                {
                    var url = urlTranca + tranca.Id + "/destrancar";

                    var teste = client.PostAsJsonAsync(url, It.IsAny<ReadTrancaDto>()).Result;
                    if (!teste.IsSuccessStatusCode)
                        throw new Exception();
                }
                var obj2 = new IntegrarBicicletaDto
                {
                    IdTranca = tranca.Id,
                    IdBicicleta = bicicletas[i].Id,
                    IdFuncionario = It.IsAny<int>()
                };
                _ = Extensions.SolicitaRequisicaoAsync<ReadBicicletaDto>(urlBike + Endpoints.Integrar,
                    HttpMethod.Post, null, null, obj2);

                i++;
            }
        }

        public void Dispose()
        {
            Console.WriteLine("End.");
            //Db.Open();

            //var cmd = new NpgsqlCommand("DELETE FROM public.\"Bicicletas\" WHERE \"Modelo\" LIKE '%Teste%';", Db);
            //_ = cmd.ExecuteNonQuery();

            //cmd = new NpgsqlCommand("DELETE FROM public.\"Trancas\" WHERE \"Modelo\" LIKE '%Teste%';", Db);
            //_ = cmd.ExecuteNonQuery();

            //cmd = new NpgsqlCommand("DELETE FROM public.\"Totems\" WHERE \"Localizacao\" LIKE '%Teste%';", Db);
            //_ = cmd.ExecuteNonQuery();

            //Db.Close();
        }
        public ReadTrancaDto GetTrancaValida()
        {
            return trancas.First(t => t.Status == EStatusTranca.OCUPADA);
        }
    }
}
