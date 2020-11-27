namespace GitHub.RepositoryAnalysis.Detectors.Utilities
{
    public class GlobalJsonModel
    {
        public SDKModel sdk { get; set; }
    }

    public class SDKModel
    {
        public string version { get; set; }
    }
}
