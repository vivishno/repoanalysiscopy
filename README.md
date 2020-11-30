# repo-analysis-detectors
Detectors for Repo Analysis service, consumed as part of Repo Analysis service. The detectors allow repo analysis service to detect what `Language` the targeted repository is written in, what will you get after building the project in that repository (`BuildTarget`), and what Azure resource does it have affinity towards, if any (`DeployTarget`).

## Get started

### Terminologies

- `Language` specifies what Language/Platform used in the repository. Currently we have support for following languages (present in repo analysis service already, not part of this library)
    - docker
    - node
    - python
    - dotnet
    - dotnetcore

- `BuildTarget` specifies what you get after building the project of particular `Language`. Following are the build targets already present in repo analysis service
    - plain (node)
    - gulp (node)
    - grunt (node)
    - aspnetcore (dotnetcore)
    - worker (dotnetcore)
    - console (dotnetcore)
    - aspnet (dotnet)
    - dotnetframeworklibrary (dotnet)
    - dotnetframeworkconsole (dotnet)
    - dotnetframeworkplain (dotnet)
    - django (python)

    Build target detectors in this library
    - react (node)
    - vue (node)

- `DeployTarget` specifies which Azure resource the project in the repo shows affinity towards (for eg, existence of `host.json` indicates the project might be Azure Functions). Following are the deploy targets already present in repo analysis service
    - azure:functions
    - Azure:AKS:HelmChart

### Pre-requisites

The library is targeting `netstandard2.0` runtime, thus make sure to have .NET Core > 2.0 on your machine,

### How to build?

Open the solution in Visual studio (2019), and build the project. Or alternatively you can navigate to the root folder with csproj file for Detectors project and run `dotnet build` from command prompt.

### How to run this project?

`Detectors` project by itself is a library, thus cannot be ran on its own. Thus we have added `TestApp`, a .NET Core web app, which will consume the `Detectors` library and use it similar to the way it will be used in the Repo analysis service.

- Build `TestApp` from Visual studio (2019) or command prompt (dotnet build), and run the project.
- This will create a server at `http://localhost:40040`.
- Make HttpPost request to `http://localhost:40040/TestRepositorAnalysis` with following payload:

```
{
  "Repository":
  {
    "Id": "<GithubUsername>/<RepoName>", // for eg, NinadKavimandan/dotnetted
    "Type": "Github", // this stays the same
    "DefaultBranch": "master", // newer repos will have main instead of master branch, so be mindful of that
    "AuthorizationInfo":{
      "Scheme":"Token", //  this stays the same
      "Parameters":{
        "AccessToken":"<YOUR_GITHUB_PAT>" // make sure the PAT has repo level access
      }
    }
  }
}
```
- You will get an output of following schema:

```

{
    "applicationSettingsList": [
        {
            "language": "<LanguageDetected>",
            "buildTargetName": "<BuildTargetDetected>",
            "deployTargetName": <DeployTargetDetected>,
            "settings": {
                "workingDirectory": "<Directory where there targets were detected>",
                ... detector particular settings
            }
        },
        ...
    ]
}

```

## Contribute

To add support for detection of new language, build target or deploy target, you will have to add detectors of respective type.
Here's how you add those:

### Adding a language detector

- All language detectors extend the abstract class `LanguageDetectorBase`, which is implementing `ILanguageDetector` interface. Thus extend this abstract class in your detector class.
- `IsLanguageDetected()` to be implemented in the new language detector you add. This is where your language detection logic part goes. This method takes a `TreeAnalysis` object as input. Here's a general model followed in the implementation of this method
```
public override bool IsLanguageDetected(TreeAnalysis treeAnalysis)
    {
        if (criteria satisfied)
        {
            return true;
        }
        return false;
    }
```
- If your language detection logic involves reading a certain file (if found) from the repository, you need to add regex to the `ImportFileToBeReadRegexes()` in `DetectorManager` class. Add the instance of this detector in the list of detectors to be returned from `ImportLanguageDetectors()` in `DetectorManager` class.

### Adding a build target detector

- A build target specifies what you get when you run the project. Projects which can be published using similar steps can be categorized into a single build target. 
- All build target detectors to extend the abstract class `BuildTargetDetectorBase`. 
- Add the instance of this build target detector in the following method, in `LanguageDetectorBase`, against the corresponding language
```
public IList<IBuildTargetDetector> GetBuildTargetDetectors()
    {
        switch (Language)
        {
            case Constants.NodeLanguageName:
                return new List<IBuildTargetDetector>
                {
                    new NodeReactBuildTargetDetector(),
                    new NodeVueBuildTargetDetector()
                };
        }
        return new List<IBuildTargetDetector> { };
    }
```
- `IsBuildTargetDetected()` and `GetBuildTargetSettings()` to be implemented in the new build detector (check `NodeReactBuildTargetDetector`/`NodeVueBuildTargetDetector`)

- `IsBuildTargetDetected()` - This is where your build target detection logic goes. This method takes a `TreeAnalysis` object as input and returns true if the detection criteria are met for a given build target.

- `GetBuildTargetSettings()` - Once the build target is detected, this method is called to generate the settings required while building the project from the repository. This method also takes a TreeAnalysis object as input, and returns a list of BuildTargetSettings objects. WorkingDirectory is a mandatory setting.

### Adding a deploy target detector

- A deploy target is the resource where the project can be published/deployed onto. A deploy target can be anything, it can be an app service, a virtual machine, containers, etc. whatever resource that can host the project.

- All deploy target detectors to extend the abstract class `DeployTargetDetectorBase`.

- Add the instance of this deploy target detector in the `GetDeployTargetDetectors` method, in `LanguageDetectorBase`, against the corresponding language. (Similar as what you did in `GetBuildTargetDetectors`)
```
    public IList<IDeployTargetDetector> GetDeployTargetDetectors()
    {
        return new List<IDeployTargetDetector> { };
    }
```

- `IsDeployTargetDetected()` and `GetDeployTargetSettings()` to be implemented in the new deploy target detector.

- `IsDeployTargetDetected()` - This is where your deploy target detection logic goes. This method takes a `TreeAnalysis` object as input and returns true if the detection criteria are met for a given deploy target.

- `GetDeployTargetSettings()` - Once the deploy target is detected, this method is called to generate the settings required while deploying the project from the repository to the detected deploy resource/target. This method also takes a `TreeAnalysis` object as input, and returns a list of `DeployTargetSettings` objects. `WorkingDirectory` is a mandatory setting and used as a key to map `DeployTargetSettings` with the `BuildTargetSettings`.

### How to test your changes?

Once you had added the detectors and added their instances to necessary functions as directed above, you just have to follow [How to run](#how-to-run-this-project)