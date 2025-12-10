# --------------------------------------------- 
# Stardust Sandbox Build Pipeline (en-US)
# ---------------------------------------------

# Clear console
Clear-Host

# Configuration
$gameName = 'StardustSandbox'
$gameVersion = 'v1.2.2.0'
$outputDir = '..\Publish'

# Project definitions
$projects = @(
    @{ Name='Windows'; Path='..\Game\SS.Windows.csproj' },
    @{ Name='Linux'; Path='..\Game\SS.Linux.csproj' }
)

# Clean output directory
if (Test-Path $outputDir) {
    Remove-Item $outputDir -Recurse -Force
}

# Function: publish + post-process
function Publish-Project {
    param (
        [string]$projectName,
        [string]$projectPath
    )

    $publishDir = Join-Path $outputDir "$gameName.$projectName.$gameVersion"
    dotnet publish $projectPath -c Release --output $publishDir
    
	if ($LASTEXITCODE -ne 0) {
        return
    }
}

# Execute publishes
foreach ($proj in $projects) {
    Publish-Project -projectName $proj.Name -projectPath $proj.Path
}

# Copy assets and remove unwanted subdirs
$assetsSource      = '..\Game\assets'
$licenseFile       = '..\..\LICENSE-ASSETS.txt'
$assetsDestination = Join-Path $outputDir "$gameName.Assets.$gameVersion\assets"
$subdirsToRemove   = @('bin','obj')

Copy-Item -Path $assetsSource -Destination $assetsDestination -Recurse -Force
Copy-Item -Path $licenseFile -Destination $assetsDestination -Force

foreach ($sub in $subdirsToRemove) {
    $path = Join-Path $assetsDestination $sub
    if (Test-Path $path) {
        Remove-Item $path -Recurse -Force
    }
}
