$buildFolder = if ($ENV:APPVEYOR_BUILD_FOLDER -eq $null) { $PSScriptRoot } else { $ENV:APPVEYOR_BUILD_FOLDER }

dotnet publish DancingGoat -c Release --runtime win-x64 -o artifacts\win-x64  -p:PublishSingleFile=true -p:PublishTrimmed=true -p:PublishReadyToRun=true --self-contained
7z a ($buildFolder + "\artifacts\kontent-sample-app-net-win-x64.zip") ($buildFolder + "\artifacts\win-x64\*.*") -r

dotnet publish DancingGoat -c Release --runtime win-x86 -o artifacts\win-x86 -p:PublishTrimmed=true -p:PublishReadyToRun=true --self-contained
7z a ($buildFolder + "\artifacts\kontent-sample-app-net-win-x86.zip") ($buildFolder + "\artifacts\win-x86\*.*") -r
