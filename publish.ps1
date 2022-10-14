dotnet publish .\DancingGoat -c Release --runtime win-x64 -o .\artifacts\win-x64  -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained
Compress-Archive ".\artifacts\win-x64\" ".\artifacts\kontent-ai-sample-app-net-win-x64.zip"
Remove-Item (".\artifacts\win-x64\" + $r) -Recurse

dotnet publish .\DancingGoat -c Release --runtime win-x86 -o .\artifacts\win-x86 -p:PublishTrimmed=true --self-contained
Compress-Archive ".\artifacts\win-x86\" ".\artifacts\kontent-ai-sample-app-net-win-x86.zip"
Remove-Item (".\artifacts\win-x86\" + $r) -Recurse