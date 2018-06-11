#requires -version 3

<#

.SYNOPSIS
This is a Powershell script to bootstrap a Cake build.

.DESCRIPTION
This Powershell script will download NuGet if missing, restore NuGet tools (including Cake)
and execute your Cake build script with the parameters you provide.

.EXAMPLE
build.ps1 -Target "Publish" -Environment Development

#>

[CmdletBinding()]
Param(
    # The build script to execute.
    [string]$Script = "publish.vsts.cake",

    # The build script target to run.
    [string]$Target = "Publish",

    # Directory for publishing artifacts
    [string]$PublishOutputDirectory = "../publish",

    # Build in release configuration
    [switch]$IsRelease
)

Write-Host "Preparing to run build script..."

$TOOLS_DIR = Join-Path $PSScriptRoot "tools"
$NUGET_EXE = Join-Path $TOOLS_DIR "nuget.exe"
$CAKE_EXE = Join-Path $TOOLS_DIR "Cake/Cake.exe"
$NUGET_URL = "https://dist.nuget.org/win-x86-commandline/v4.1.0/nuget.exe"

# Make sure tools folder exists
New-Item -Path $TOOLS_DIR -Type directory -Force | out-null

# Try download NuGet.exe if not exists
if (!(Test-Path $NUGET_EXE)) {
    Write-Verbose -Message "Downloading NuGet.exe..."
    try {
        Invoke-WebRequest -Uri $NUGET_URL -OutFile $NUGET_EXE
    } catch {
        Throw "Could not download NuGet.exe."
    }
}

# Save nuget.exe path to environment to be available to child processed
$ENV:NUGET_EXE = $NUGET_EXE

# Restore Cake from NuGet if does not exist
if(!(Test-Path $CAKE_EXE)) {
    Write-Verbose -Message "Restoring Cake from NuGet..."
    $NuGetOutput = Invoke-Expression "&`"$NUGET_EXE`" install Cake -ExcludeVersion -Version 0.26.1 -OutputDirectory `"$TOOLS_DIR`""

    if ($LASTEXITCODE -ne 0) {
        Throw "An error occured while restoring Cake."
    }
    Write-Verbose -Message ($NuGetOutput | out-string)
}

# Start Cake
Write-Host "Running build script..."

& "$CAKE_EXE" "$Script" -target="$Target" -isRelease="$IsRelease" -publishOutputDirectory="$PublishOutputDirectory" -Experimental
exit $LASTEXITCODE