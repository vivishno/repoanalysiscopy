// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DockerFileParser.cs" company="Microsoft Corporation">
//   2012-2023, All rights reserved.
// </copyright>
// <summary>
//    Parser class for Dockerfile.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GitHub.RepositoryAnalysis.Detectors.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class CommandModel
    {
        public DockerCommand command;
        public Dictionary<string, List<object>> args;
    }

    enum DockerCommand
    {
        RUN = 0,
        EXPOSE = 1
    }

    public class DockerFileParser
    {
        private string contents;
        private List<CommandModel> dockerFile = new List<CommandModel>();

        public DockerFileParser(string contents)
        {
            this.contents = contents;
            try
            {
                this.Parse();
            }
            catch (Exception)
            {
                this.dockerFile = null;
            }
        }

        private void Parse()
        {
            string[] lines = this.contents.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            List<String> filteredLines = new List<String>();
            string lineUnderConsideration = string.Empty;

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (String.IsNullOrEmpty(trimmedLine) || this.isComment(trimmedLine))
                {
                    continue;
                }

                if (trimmedLine.EndsWith("\\"))
                {
                    lineUnderConsideration += trimmedLine.Remove(trimmedLine.Length - 1, 1);
                }
                else
                {
                    lineUnderConsideration += trimmedLine;
                    filteredLines.Add(lineUnderConsideration);
                    lineUnderConsideration = "";
                }
            }

            this.ParseCommands(filteredLines);
        }

        private void ParseCommands(List<string> filteredLines)
        {
            List<CommandModel> modals = new List<CommandModel>();
            foreach (var line in filteredLines)
            {
                string[] commandsSplit = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                switch(commandsSplit[0].ToUpperInvariant())
                {
                    case "EXPOSE":
                        CommandModel m = new CommandModel();
                        m.command = DockerCommand.EXPOSE;
                        Dictionary<string, List<object>> args = new Dictionary<string, List<object>>();
                        for (int i = 1; i < commandsSplit.Length; i++)
                        {
                            if (!args.ContainsKey("ports"))
                            {
                                args.Add("ports", new List<object>());
                            }

                            args["ports"].Add(commandsSplit[i]);
                        }
                        m.args = args;
                        modals.Add(m);
                        break;
                    default:
                        break;
                }
            }
            this.dockerFile = modals;
        }

        public List<string> GetPortNumbers()
        {
            List<string> portNumbers = new List<string>();
            foreach(CommandModel command in this.dockerFile)
            {
                if(command.command == DockerCommand.EXPOSE)
                {
                    portNumbers.AddRange(command.args["ports"].Select(port => (string)port));
                }
            }
            return portNumbers;
        }

        bool isComment(string trimmedLine)
        {
            return trimmedLine.StartsWith("#");
        }
    }
}
