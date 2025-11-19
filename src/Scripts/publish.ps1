# --------------------------------------------- 
# Stardust Sandbox Build Pipeline (en-US)
# ---------------------------------------------

# Clear console
Clear-Host

# Configuration
$gameName      = 'StardustSandbox'
$gameVersion   = 'v0.0.0.0'
$outputDir     = '..\Publish'
$beauty2Dir    = 'libraries'
$beauty2Ignore = 'SDL2*;libSDL2*;sdl2*;soft_oal*;openal*;libopenal*;'
$beauty2Hiddens = 'StardustSandbox.dll;hostfxr;hostpolicy;*.deps.json;*.runtimeconfig*.json;'

# Project definitions
$projects = @(
    @{ Name='windowsdx'; Path='..\StardustSandbox\StardustSandbox.csproj'; Runtimes=@('win-x64') }
)

# Clean output directory
if (Test-Path $outputDir) {
    Remove-Item $outputDir -Recurse -Force
}

# Function: publish + post-process
function Publish-Project {
    param (
        [string]$projectName,
        [string]$projectPath,
        [string]$runtime
    )

    $publishDir = Join-Path $outputDir "$gameName.$gameVersion.$projectName.$runtime"
    dotnet publish $projectPath -c Release -r $runtime --output $publishDir
    
	if ($LASTEXITCODE -ne 0) {
        return
    }
}

# Execute publishes
foreach ($proj in $projects) {
    foreach ($rt in $proj.Runtimes) {
        Publish-Project -projectName $proj.Name -projectPath $proj.Path -runtime $rt
    }
}

# Copy assets and remove unwanted subdirs
$assetsSource      = '..\StardustSandbox\assets'
$licenseFile       = '..\..\LICENSE-ASSETS.txt'
$assetsDestination = Join-Path $outputDir "$gameName.$gameVersion.assets\assets"
$subdirsToRemove   = @('bin','obj')

Copy-Item -Path $assetsSource -Destination $assetsDestination -Recurse -Force
Copy-Item -Path $licenseFile -Destination $assetsDestination -Force

foreach ($sub in $subdirsToRemove) {
    $path = Join-Path $assetsDestination $sub
    if (Test-Path $path) {
        Remove-Item $path -Recurse -Force
    }
}
