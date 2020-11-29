namespace TestApp
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.Services.PortalExtension.RepositoryAnalysis.WebApi.Contracts;
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
