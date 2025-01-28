# ================================================= #
# Stardust Sandbox Build Pipeline                   #
# ================================================= #

# Clear the console window
Clear-Host

# General
$gameName = "StardustSandbox"
$gameVersion = "v0.0.0.0"

# beauty2
$beauty2Directory = "libraries"
$beauty2Ignored = "SDL2.dll;libSDL2-2.0.so.0;libopenal.so.1;libopenal.1.dylib;libSDL2.dylib;soft_oal.dll;"

# Define solutions and publishing directories
$windowsDX = "..\..\SS.Game\StardustSandbox.WindowsDX.Game.csproj"
$desktopGL = "..\..\SS.Game\StardustSandbox.DesktopGL.Game.csproj"
$outputDirectory = "..\..\Publish"

if (Test-Path $outputDirectory) {
    Remove-Item -Path $outputDirectory -Recurse -Force
    Write-Output "We found a Publish directory. To avoid potential failures, it was deleted for the rebuild process."
}

# List of target platforms
$platforms = @("win-x64", "linux-x64", "osx-x64")

# Function to publish a project for a given platform
function Publish-Project($projectName, $projectPath, $platform) {
    $publishDir = "$outputDirectory\$gameName.$gameVersion.$projectName.$platform"

    Write-Host "Publishing $projectPath for $platform..."
    dotnet publish $projectPath -c Release -r $platform --output $publishDir
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Publishing failed for $platform."
        return
    }

    # Organize the published output
    nbeauty2 --usepatch --loglevel Detail $publishDir $beauty2Directory $beauty2Ignored
    Write-Host "Publishing and organization for $platform completed."
}

# Function to delete specified subdirectories
function Remove-Subdirectories($destination, $subdirectoriesToDelete) {
    foreach ($subdirectory in $subdirectoriesToDelete) {
        $subdirectoryPath = Join-Path -Path $destination -ChildPath $subdirectory
        if (Test-Path $subdirectoryPath -PathType Container) {
            Remove-Item -Path $subdirectoryPath -Recurse -Force
            Write-Host "Subdirectory $subdirectory deleted successfully."
        } else {
            Write-Host "Subdirectory $subdirectory not found in $destination."
        }
    }
}

# Delete existing directories
if (Test-Path $outputDirectory -PathType Container) {
    Remove-Item -Path $outputDirectory -Recurse -Force
    Write-Host "Existing directory deleted."
}

# Publish WindowsDX
Write-Host "Publishing Stardust Sandbox (WindowsDX) for Win-x64..."
Publish-Project "windowsdx" $windowsDX $platforms[0]

# Publish DesktopGL for all platforms
Write-Host "Publishing Stardust Sandbox (DesktopGL) for all platforms..."
foreach ($platform in $platforms) {
    Publish-Project "desktopgl" $desktopGL $platform
    Write-Host "Next..."
}

Write-Host "All publishing processes have been completed."

# Copy assets directory and delete specific subdirectories
Write-Host "Copying assets directory..."

$source = "..\..\SS.ContentBundle\assets"
$license = "..\..\..\LICENSE-ASSETS.txt"
$destination = "$outputDirectory\$gameName.$gameVersion.assets\assets"
$subdirectoriesToDelete = @("bin", "obj")

# Copy the source folder to the destination
Copy-Item -Path $source -Destination $destination -Recurse
Copy-Item -Path $license -Destination $destination

# Delete the specified subdirectories
Remove-Subdirectories $destination $subdirectoriesToDelete

Write-Host "Operation completed. The folder has been copied to $destination, and the specified subdirectories have been deleted."
