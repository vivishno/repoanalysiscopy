namespace TestApp
{
    using GitHub.RepositoryAnalysis.Detectors.Helpers;
    using GitHub.RepositoryAnalysis.Detectors.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using TestApp.Models;
    using SourceRepository = Models.SourceRepository;

    public class TreeAnalysisService
    {
        private static readonly HttpClient client = new HttpClient();
        static string baseUrl = "https://api.github.com:443/repos";

        public async Task<TreeAnalysis> GetTreeAnalysis(SourceRepository sourceRepository)
        {
            TreeAnalysis treeAnalysis = new TreeAnalysis();

            IList<FileSystemTreeNode> repositoryTree = await GetRepositoryTree(sourceRepository);
            await ConvertGitTreeToTreeAnalysis(repositoryTree, sourceRepository, treeAnalysis);

            return treeAnalysis;
        }

        private async Task ConvertGitTreeToTreeAnalysis(IList<FileSystemTreeNode> repositoryTree, SourceRepository sourceRepository, TreeAnalysis treeAnalysis)
        {
            Dictionary<string, IList<FileSystemTreeNode>> fileInfo = new Dictionary<string, IList<FileSystemTreeNode>>(StringComparer.InvariantCultureIgnoreCase);
            Dictionary<string, IList<FileSystemTreeNode>> directoryInfo = new Dictionary<string, IList<FileSystemTreeNode>>(StringComparer.InvariantCultureIgnoreCase);
            HashSet<string> fileExtensions = new HashSet<string>();
            HashSet<string> filesToRead = new HashSet<string>();
            List<string> regexPatterns = new List<string>();
            regexPatterns.AddRange(new DetectorManager().ListPatternsForFilesToBeRead());
            foreach (FileSystemTreeNode node in repositoryTree)
            {
                if (!node.IsDirectory)
                {
                    if (fileInfo.ContainsKey(node.Name))
                    {
                        fileInfo[node.Name.ToLower()].Add(node);
                    }
                    else
                    {
                        IList<FileSystemTreeNode> nodeList = new List<FileSystemTreeNode>();
                        nodeList.Add(node);
                        fileInfo[node.Name.ToLower()] = nodeList;
                    }

                    if (!string.IsNullOrEmpty(Path.GetExtension(node.Name)))
                    {
                        fileExtensions.Add(Path.GetExtension(node.Name).ToLower());
                    }

                    foreach (var regex in regexPatterns)
                    {
                        if (Regex.IsMatch(node.Path, regex, RegexOptions.IgnoreCase))
                        {
                            filesToRead.Add(node.Path);
                        }
                    }
                }
                else
                {
                    if (!directoryInfo.ContainsKey(node.Name))
                    {
                        directoryInfo[node.Name.ToLower()] = new List<FileSystemTreeNode>();
                    }
                    directoryInfo[node.Name.ToLower()].Add(node);
                }
            }

            Dictionary<string, FileContent> filesContent = await ReadFilesFromGithub(sourceRepository, filesToRead);

            treeAnalysis.DirectoryInfo = directoryInfo;
            treeAnalysis.FileExtensions = fileExtensions;
            treeAnalysis.FilesContent = filesContent;
            treeAnalysis.FilesInfo = fileInfo;
        }

        private async Task<Dictionary<string, FileContent>> ReadFilesFromGithub(SourceRepository sourceRepository, HashSet<string> filesToRead)
        {
            Dictionary<string, FileContent> filesContent = new Dictionary<string, FileContent>();
            foreach (var filePath in filesToRead)
            {
                try
                {
                    var value = await GetFileContent(sourceRepository, filePath);
                    if (!string.IsNullOrEmpty(value))
                    {
                        filesContent[filePath] = new FileContent { Value = value };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return filesContent;
        }

        private async Task<string> GetFileContent(SourceRepository sourceRepository, string filePath)
        {
            string fileContentUrl = $"{baseUrl}/{sourceRepository.Repository.Id}/contents/{filePath}";
            var fileContent = await GetHttpRequest<GitFileContent>(fileContentUrl, sourceRepository.Repository.AuthorizationInfo.Parameters["AccessToken"]);

            byte[] contentBytes = Convert.FromBase64String(fileContent.Content);
            string content = Encoding.UTF8.GetString(contentBytes);

            return content;
        }

        private async Task<IList<FileSystemTreeNode>> GetRepositoryTree(SourceRepository sourceRepository)
        {
            IList<FileSystemTreeNode> repositoryTree = new List<FileSystemTreeNode>();

            string repoTreeUrl = $"{baseUrl}/{sourceRepository.Repository.Id}/git/trees/{sourceRepository.Repository.DefaultBranch}?recursive=1";
            var response = await GetHttpRequest<TreeResponse>(repoTreeUrl, sourceRepository.Repository.AuthorizationInfo.Parameters["AccessToken"]);

            foreach (var node in response.Tree)
            {
                repositoryTree.Add(new FileSystemTreeNode { Path = node.Path, Name = node.Path.Split(new String[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Last(), IsDirectory = String.Equals(node.Type, "tree", StringComparison.OrdinalIgnoreCase)  });
            }

            return repositoryTree;
        }

        private async Task<T> GetHttpRequest<T>(string url, string accessToken) where T : class
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.GitHub.V3+json");
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.speedy-preview+json");
                client.DefaultRequestHeaders.Add("Authorization", $"token {accessToken}");
                client.DefaultRequestHeaders.Add("User-Agent", "TestApp");

                try
                {
                    var resultTask = client.GetStreamAsync(url);
                    var response = await JsonSerializer.DeserializeAsync<T>(await resultTask);
                    return response;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
