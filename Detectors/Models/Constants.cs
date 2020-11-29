namespace GitHub.RepositoryAnalysis.Detectors.Models
{
    public static class Constants
    {
        //RepositoryAnalysis Constants
        public const string BuildTarget = "BuildTarget";
        public const string DeployTarget = "DeployTarget";
        public const string Language = "Language";
        public const string WorkingDirectory = "workingDirectory";
        public const string PlainApplication = "plain";

        //RepositoryAnalysis Python Constants
        public const string PythonLanguageName = "python";
        public const string PythonFileExtension = ".py";
        public const string DjangoBuildTargetName = "django";
        public const string DjangoSettingModuleName = "settings.py";
        public const string DjangoSettingModulePath = "djangoSettingModulePath";
        public const string ManageFilePath = "manageFilePath";
        public const string ManageFileName = "manage.py";
        public const string RequirementsFilePath = "requirementsFilePath";
        public const string RequirementsFileName = "requirements.txt";
        public const string SetupFilePath = "setupFilePath";
        public const string SetupFileName = "setup.py";

        //RepositoryAnalysis AzureFunction constants
        public const string AzureFunctionName = "azure:functions";
        public const string HostFilePath = "hostFilePath";
        public const string HostFileName = "host.json";


        //RepositoryAnalysis Node Constants
        public const string NodeLanguageName = "node";
        public const string GulpTaskRunner = "gulp";
        public const string GruntTaskRunner = "grunt";
        public const string PackageJsonFilePath = "packageFilePath";
        public const string PackageJsonFileName = "package.json";
        public const string GulpFilePath = "gulpFilePath";
        public const string GulpFileName = "gulpfile.js";
        public const string GruntFilePath = "gruntFilePath";
        public const string GruntFileName = "gruntfile.js";
        public const string ReactBuildTargetName = "react";
        public const string AppLocation = "appLocation";
        public const string AppArtifactLocation = "appArtifactLocation";
        public const string ReactBuildArtifactPath = "build";
        public const string ReactPackageName = "react";
        public const string VuePackageName = "vue";
        public const string VueBuildTargetName = "vue";
        public const string VueBuildArtifactPath = "dist";

        //RepositoryAnalysis Docker constants
        public const string Docker = "docker";
        public const string Dockerfile = "dockerfile";
        public const string DockerFilePath = "dockerFilePath";
        public const string DockerPort = "dockerPort";

        //RepositoryAnalysis AKSHelmChart constants
        public const string AKSHelmChart = "Azure:AKS:HelmChart";
        public const string chartFileName = "chart.yaml";
        public const string helmChartPath = "helmChartPath";
        public const string templatesDirectoryName = "templates";

        // API version
        public const string apiVersion201907 = "2019-07-01-preview";
        public const string apiVersion202007 = "2020-07-13-preview";

        //RepositoryAnalysis DotNetCore Constants
        public const string DotNetProjectExtension = ".csproj";
        public const string CsProjDetectionRegex = "^.*\\.csproj?$";
        public const string DotNetCore = "dotnetcore";
        public const string GlobalJsonParseRegex = "^.*global\\.json";
        public const string GlobalJsonFileName = "global.json";
        public const string SdkVersion = "sdkVersion";
        public const string CsprojPath = "csprojPath";
        public const string TargetFrameworksTag = "targetframeworks";
        public const string TargetFrameworkTag = "targetframework";
        public const string OutputTypeTag = "outputtype";
        public const string SdkTag = "sdk";
        public const string DotNetCoreApplication = "dotnetcoreapplication";
        public const string DotNetFrameworkApplication = "dotnetapplication";
        public const string DotNetCoreWebBuildTargetName = "aspnetcore";
        public const string DotNetCoreWorkerBuildTargetName = "worker";
        public const string DotNetCoreConsoleBuildTargetName = "console";
        public const string DotNetCoreWebSdkName = "microsoft.net.sdk.web";
        public const string DotNetCoreWorkerSdkName = "microsoft.net.sdk.worker";
        public const string DotNetCoreConsoleOutputType = "exe";
        public const string LanguageVersion = "languageVersion";
        public const string DotNetCorePrefix = "netcoreapp";

        //RepositoryAnalysis DotNetFx Constants
        public const string DotNetFramework = "dotnet";
        public const string TargetFrameworkVersionTag = "targetframeworkversion";
        public const string DotNetFrameworkPlainBuildTarget = "dotnetframeworkplain";
        public const string DotNetFrameworkWebBuildTarget = "aspnet";
        public const string DotNetFrameworkConsoleBuildTarget = "dotnetframeworkconsole";
        public const string WebConfigFileName = "Web.config";
        public const string DotNetFrameworkLibraryBuildTarget = "dotnetframeworklibrary";
        public const string DotNetFrameworkLibraryOutputType = "library";
        public const string SolutionFileExtension = ".sln";
        public const string SolutionDirectory = "solutionDirectory";
        public const string DotNetFrameworkPrefix = "net";
        public const string DotNetStandardPrefix = "netstandard";
        public const string AngularBuildTargetName = "angular";
        public const string AngularBuildArtifactPath = "dist";
        public const string AngularPackageName = "@angular/core";
    }
}
