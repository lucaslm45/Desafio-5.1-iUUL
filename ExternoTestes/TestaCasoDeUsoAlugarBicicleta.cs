using Externo.Util;
using ExternoTestes.Equipamento;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Xunit.Abstractions;

namespace ExternoTestes
{
    public class TestaCasoDeUsoAlugarBicicleta : IClassFixture<EquipServiceTestFixture>
    {
        private readonly WebApplicationFactory<Program> application;
        private readonly HttpClient client;
        private readonly ITestOutputHelper output;

        EquipServiceTestFixture fixture;

        public TestaCasoDeUsoAlugarBicicleta(ITestOutputHelper output, EquipServiceTestFixture fixture)
        {

            application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {

                });

            client = application.CreateClient();

            this.output = output;
            this.fixture = fixture;
        }

        [Fact]
        public void TestaCaminhoPrincipalAsync()
        {
            //Arrange
            var tranca = fixture.GetTrancaValida();
            output.WriteLine(tranca.Id + "\t" + tranca.Status.ToString());
        }
    }
}
