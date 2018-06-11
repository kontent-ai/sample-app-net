var target = Argument<string>("target", "Publish");
var publishOutputDirectory = Argument<string>("publishOutputDirectory", "../publish");

/* 
* Builds
*/

Task("Publish")
    .Does(() =>
{
    const string ProjectFile = "../DancingGoat/DancingGoat.csproj";
    var projectName = System.IO.Path.GetFileNameWithoutExtension(ProjectFile);
    var publishPath = $"{publishOutputDirectory}/{projectName}";
    var publishArchive = $"{publishPath}.zip";
    var tempPublishDirectory = MakeAbsolute(Directory(publishPath));    

    CreateDirectory(tempPublishDirectory);
    WebDeployToFolder(ProjectFile, tempPublishDirectory.FullPath);

	  Zip(tempPublishDirectory, publishArchive);
    DeleteDirectory(tempPublishDirectory, recursive: true);
    Information($"##vso[artifact.upload containerfolder={projectName};artifactname={projectName};]{MakeAbsolute(File(publishArchive))}");
});


private void WebDeployToFolder(string projectFile, string publishDirectory)
{
    var solutionDir = MakeAbsolute(Directory("../"));

    MSBuild(projectFile, settings =>
        settings.SetConfiguration("Debug")
            .SetVerbosity(Cake.Core.Diagnostics.Verbosity.Minimal)
            .WithTarget("Build;WebPublish")
            .WithProperty("WebPublishMethod","FileSystem")
            .WithProperty("publishUrl", publishDirectory)
            .WithProperty("SolutionDir", solutionDir.FullPath)
            .WithProperty("DeployAPI", "true")
            .WithProperty("ProfileTransformWebConfigEnabled", "false")
        );
}

RunTarget(target);