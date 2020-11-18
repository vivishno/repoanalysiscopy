namespace GitHub.Services.RepositoryAnalysis.Detectors.Models
{
    using System;
    using System.Linq;

    /// <summary>
    /// Node in the TreeAnalysis object.
    /// </summary>
    public class FileSystemTreeNode
    {
        /// <summary>
        /// File name including its extension
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Full path including file name
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// true if the node specifies a directory.
        /// </summary>
        public bool IsDirectory { get; set; }

        /// <summary>
        /// Finds parent directory name for the node from treeanalysis object.
        /// </summary>
        /// <returns>Parent directory name</returns>
        public string GetDirectoryPath()
        {
            string nodePath = this.Path;
            if (string.IsNullOrEmpty(System.IO.Path.GetDirectoryName(nodePath)))
            {
                return ".";
            }
            string[] dirs = nodePath.Split('/');
            return String.Join("/", dirs.Take(dirs.Length - 1));
        }
    }
}
