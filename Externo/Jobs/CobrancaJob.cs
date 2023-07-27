using Externo.Controllers;
using Quartz;

namespace Externo.Jobs
{
    public class CobrancaJob : IJob
    {
        private readonly CobrancaController _controller;

        public CobrancaJob(CobrancaController controller)
        {
            _controller = controller;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _controller.ProcessaCobrancasEmFila();
        }
    }
}
