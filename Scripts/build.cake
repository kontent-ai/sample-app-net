var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

/* 
* Builds
*/

Task("Publish")
    .IsDependentOn("CleanDirs")
    .IsDependentOn("Build")
    .IsDependentOn("PublishInternal");

/*
* Tasks
*/

TaskSetup(context =>
{
	OnTeamCityOnly(() => TeamCity.WriteStartBlock(context.Task.Name));
});

TaskTeardown(context =>
{
    OnTeamCityOnly(() => TeamCity.WriteEndBlock(context.Task.Name));
});

Task("CleanDirs")
    .Does(() =>
{
    CleanDirectory(PublishOutputDirectory);
});

Task("Build")
    .IsDependentOn("CleanDirs")
    .Does(() =>
{
    const string solutionFile = "../DancingGoat.sln";
    NuGetRestore(solutionFile);
    MSBuild(solutionFile, settings =>
        settings.SetConfiguration(configuration)
            .SetVerbosity(Verbosity.Minimal));
});

Task("PublishInternal")
    .Does(() =>
{
    const string ProjectFile = "../DancingGoat/DancingGoat.csproj";
    var projectName = System.IO.Path.GetFileNameWithoutExtension(ProjectFile);
    var publishPath = $"{PublishOutputDirectory}/{projectName}";
    var publishArchive = $"{publishPath}.zip";
    var tempPublishDirectory = MakeAbsolute(Directory(publishPath));    

    CreateDirectory(tempPublishDirectory);
    BuildSolution(ProjectFile, tempPublishDirectory.FullPath);

	Zip(tempPublishDirectory, publishArchive);
    DeleteDirectory(tempPublishDirectory, recursive: true);

    OnTeamCityOnly(() => TeamCity.PublishArtifacts(MakeAbsolute(File(publishArchive)).FullPath));
});

/*
* Helper methods and constants
*/

const string PublishOutputDirectory = "../publish";

private void BuildSolution(string projectFile, string publishDirectory)
{
    var solutionDir = MakeAbsolute(Directory("../"));

    MSBuild(projectFile, settings =>
        settings.SetConfiguration(configuration)
            .SetVerbosity(Cake.Core.Diagnostics.Verbosity.Minimal)
            .WithTarget("Build;WebPublish")
            .WithProperty("WebPublishMethod","FileSystem")
            .WithProperty("publishUrl", publishDirectory)
            .WithProperty("SolutionDir", solutionDir.FullPath)
            .WithProperty("DeployAPI", "true")
            .WithProperty("ProfileTransformWebConfigEnabled", "false")
        );
}

private void OnTeamCityOnly(Action action) 
{
  if(TeamCity.IsRunningOnTeamCity)
    action();
}

RunTarget(target);