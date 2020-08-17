dotnet tool restore
$outputFolder = $PSScriptRoot + '\..\Models\ContentTypes'
dotnet tool run KontentModelGenerator -p "975bf280-fd91-488c-994c-2f04416e5ee3" -o $outputFolder -n "DancingGoat.Models" -g True -s True