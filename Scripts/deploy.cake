#addin "Cake.MsDeploy"

var target = Argument<string>("target", "Default");
var publishSettings = Argument("publishSettings", "");
var zipPath = "../publish/DancingGoat.zip";

/* 
* Builds
*/

Task("Deploy")
    .WithCriteria(!String.IsNullOrEmpty(publishSettings) && FileExists(publishSettings))
    .WithCriteria(!String.IsNullOrEmpty(zipPath) && FileExists(zipPath))
    .Does(() =>
{
    var publishProfile = GetPublishProfile(publishSettings);
    
    Information($"Deploying to: {publishProfile.MsDeployURL}");
    DeployArchive(zipPath, publishProfile);
});

/*
* Tasks
*/

TaskSetup(context =>
{
  if (TeamCity.IsRunningOnTeamCity)
    TeamCity.WriteStartBlock(context.Task.Name);
});

TaskTeardown(context =>
{
  if (TeamCity.IsRunningOnTeamCity)
    TeamCity.WriteEndBlock(context.Task.Name);
});

/*
* Helper methods and constants
*/

private void DeployArchive(string zipFile, PublishProfile publishProfile) 
{
    MsDeploy(new MsDeploySettings
    {
        Verb = Operation.Sync,
        RetryAttempts = 5,
        RetryInterval = 5000,
        Source = new PackageProvider
        {
            Direction = Direction.source,
            Path = MakeAbsolute(File(zipFile)).FullPath
        },
        Destination = new AutoProvider
        {
            Direction = Direction.dest,
            IncludeAcls = false,
            AuthenticationType = AuthenticationScheme.Basic,
            Username = publishProfile.UserName,
            Password = publishProfile.Password,
            ComputerName = publishProfile.MsDeployURL,            
        },
        EnableRules = new[] { "AppOffline" }
    });
}

private class PublishProfile
{
    public string UserName { get; private set; }
    public string Password { get; private set; }
    public string MsDeployURL { get; private set; }
    
    public PublishProfile(string userName, string password, string msdeployURL)
    {
        UserName = userName;
        Password = password;
        MsDeployURL = msdeployURL;
    }
}

private string TrimStart(string source, string prefix)
    => (!source.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase)) 
        ? source
        : source.Substring(prefix.Length);

private PublishProfile GetPublishProfile(string publishSettings)
{
    const string baseProfilePath = "//publishData/publishProfile[@publishMethod='MSDeploy']/{0}";
    var publishUrl = XmlPeek(publishSettings, String.Format(baseProfilePath, "@publishUrl"));
    var userName = XmlPeek(publishSettings, String.Format(baseProfilePath, "@userName"));
    var site = XmlPeek(publishSettings, String.Format(baseProfilePath, "@msdeploySite"));
    var password = XmlPeek(publishSettings, String.Format(baseProfilePath, "@userPWD"));
    
    publishUrl = TrimStart(publishUrl, "https://");
    publishUrl = TrimStart(publishUrl, "http://");
    var msdeployURL = String.Format("https://{0}/msdeploy.axd?site={1}", publishUrl, site);    
    
    return new PublishProfile(userName, password, msdeployURL);
}

RunTarget(target);