#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=OpenCover"
#tool "nuget:?package=ReportGenerator"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

/* 
* Builds
*/

Task("Publish")
    .IsDependentOn("CleanDirs")
    .IsDependentOn("Build")
    .IsDependentOn("Tests")
    .IsDependentOn("PublishInternal");

Task("PublishWithoutTests")
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
    CleanDirectory(ReportOutputDirectory);
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
            .UseToolVersion(MSBuildToolVersion.VS2015)
            .SetVerbosity(Verbosity.Minimal));
});

Task("Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var resultXmlPath = $"{ReportOutputDirectory}/result.xml";
    var coverageZipPath = $"{ReportOutputDirectory}/coverage.zip";

    CreateDirectory(ReportOutputDirectory);    
    CreateDirectory(CoverageOutputDirectory);

    RunCodeCoverage(resultXmlPath);
    GenerateReport(resultXmlPath);
    
    OnTeamCityOnly(() =>
    {
        Zip(CoverageOutputDirectory, coverageZipPath);
        TeamCity.PublishArtifacts(MakeAbsolute(File(coverageZipPath)).FullPath);
    });
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

const string ReportOutputDirectory = "../reports";
const string PublishOutputDirectory = "../publish";
var CoverageOutputDirectory = $"{ReportOutputDirectory}/coverage";

private void BuildSolution(string projectFile, string publishDirectory)
{
    var solutionDir = MakeAbsolute(Directory("../"));

    MSBuild(projectFile, settings =>
        settings.SetConfiguration(configuration)
            .SetVerbosity(Cake.Core.Diagnostics.Verbosity.Minimal)
            .UseToolVersion(MSBuildToolVersion.VS2015)
            .WithTarget("Build;WebPublish")
            .WithProperty("WebPublishMethod","FileSystem")
            .WithProperty("publishUrl", publishDirectory)
            .WithProperty("SolutionDir", solutionDir.FullPath)
            .WithProperty("DeployAPI", "true")
            .WithProperty("ProfileTransformWebConfigEnabled", "false")
        );
}

private void GenerateReport(string resultXmlPath)
{
    ReportGenerator(resultXmlPath, CoverageOutputDirectory, new ReportGeneratorSettings() 
    {
        ReportTypes = new List<ReportGeneratorReportType>() { ReportGeneratorReportType.Badges, ReportGeneratorReportType.Html }
    });
}

private void RunCodeCoverage(string resultXmlPath)
{
    var testAssemblies = GetFiles("../Tests/Tests.Unit.Tests/bin/**/*.Unit.Tests.dll");
    var xUnit2Settings = new XUnit2Settings {
        Parallelism = ParallelismOption.All,
        HtmlReport = true,
        NoAppDomain = false,
        NUnitReport = true,
        ShadowCopy = false,
        OutputDirectory = ReportOutputDirectory,
        ArgumentCustomization = args => args.Append(TeamCity.IsRunningOnTeamCity ? "-teamcity" : "")
    };

    var openCoverSettings = new OpenCoverSettings() { SkipAutoProps = true }
        .WithFilter("+[Kentico.Draft.*]*")
        .WithFilter("-[Kentico.Draft.Tests.*]*")
        .ExcludeByAttribute("*ExlcudeFromCodeCover*")
        .ExcludeByAttribute("*GeneratedCode*");

    OpenCover(
        tool => tool.XUnit2(testAssemblies, xUnit2Settings),
        new FilePath(resultXmlPath),
        openCoverSettings
    );
}

private void OnTeamCityOnly(Action action) 
{
  if(TeamCity.IsRunningOnTeamCity)
    action();
}

RunTarget(target);