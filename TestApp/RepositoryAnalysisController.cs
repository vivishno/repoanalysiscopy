namespace TestApp
{
    using System.Threading.Tasks;
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using Microsoft.AspNetCore.Mvc;
    using SourceRepository = Models.SourceRepository;

    [ApiController]
    [Route("[controller]")]
    public class TestRepositorAnalysisController : ControllerBase
    {
        [HttpPost]
        public async Task<RepositoryAnalysis> GetRepositoryAnalysis([FromBody] SourceRepository sourceRepository)
        {
            RepositoryAnalysisService service = new RepositoryAnalysisService();
            return await service.Analyze(sourceRepository);
        }
    }
}
