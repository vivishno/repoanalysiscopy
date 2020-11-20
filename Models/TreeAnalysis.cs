namespace GitHub.Services.RepositoryAnalysis.Detectors.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Output of after analysis of git tree.
    /// Encapsulates information about dictionaries, files, extensions and contents of the files read.
    /// </summary>
    public class TreeAnalysis
    {
        /// <summary>
        /// Dictionary of file name mapped against nodes with same name in the repo.
        /// </summary>
        public Dictionary<string, IList<FileSystemTreeNode>> FilesInfo { get; set; }

        /// <summary>
        /// Set of all extensions found in the repo.
        /// </summary>
        public HashSet<string> FileExtensions { get; set; }

        /// <summary>
        /// Dictionary of file name against the contents as FileContent object.
        /// </summary>
        public Dictionary<string, FileContent> FilesContent { get; set; }

        /// <summary>
        /// Dictionary of directory name against nodes with same name in the repo.
        /// </summary>
        public Dictionary<string, IList<FileSystemTreeNode>> DirectoryInfo { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public TreeAnalysis()
        {
            FilesInfo = new Dictionary<string, IList<FileSystemTreeNode>>();
            FileExtensions = new HashSet<string>();
            FilesContent = new Dictionary<string, FileContent>();
            DirectoryInfo = new Dictionary<string, IList<FileSystemTreeNode>>();
        }
    }
}
