namespace GitHub.RepositoryAnalysis.Detectors.Models
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

        /// <summary>
        /// Checks if current node is an ancestor of second parameter node.
        /// </summary>
        /// <param name="child"></param>
        /// <returns>true if current node is indeed an ancestor of "child"</returns>
        public bool IsAncestorOf(FileSystemTreeNode child)
        {
            return (System.IO.Path.GetDirectoryName(child.Path) + System.IO.Path.DirectorySeparatorChar)
                .Contains(System.IO.Path.GetDirectoryName(this.Path) + System.IO.Path.DirectorySeparatorChar);
        }
    }
}
