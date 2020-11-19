# repo-analysis-detectors
Detectors for Repo Analysis service, consumed as part of PortalExtension service in AzureDevOps. The library is targeting `netstandard2.0` runtime.

## Contribute

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
