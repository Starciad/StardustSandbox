# --------------------------------------------- 
# Stardust Sandbox Build Pipeline (en-US)
# ---------------------------------------------

# Clear console
Clear-Host

# Configuration
$gameName      = 'StardustSandbox'
$gameVersion   = 'v0.0.0.0'
$outputDir     = '..\..\Publish'
$beauty2Dir    = 'libraries'
$beauty2Ignore = 'SDL2*;libSDL2*;sdl2*;soft_oal*;openal*;libopenal*;'

# Project definitions
$projects = @(
    @{ Name='windowsdx'; Path='..\..\Projects\SS.Game\StardustSandbox.WindowsDX.Game.csproj'; Runtimes=@('win-x64') }
)

# Clean output directory
if (Test-Path $outputDir) {
    Write-Host "Removing existing publish directory: $outputDir"
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
    Write-Host "→ Publishing '$projectName' for runtime '$runtime'..."
    dotnet publish $projectPath -c Release -r $runtime --output $publishDir
    if ($LASTEXITCODE -ne 0) {
        Write-Error "✖ Publish failed for $projectName/$runtime."
        return
    }

    # Run nbeauty2 to strip ignored files
    nbeauty2 --usepatch --loglevel Detail $publishDir $beauty2Dir $beauty2Ignore
    Write-Host "✔ Publish & cleanup completed for '$projectName' ($runtime)."
}

# Execute publishes
foreach ($proj in $projects) {
    foreach ($rt in $proj.Runtimes) {
        Publish-Project -projectName $proj.Name -projectPath $proj.Path -runtime $rt
    }
}

Write-Host "All publish processes completed.`n"

# Copy assets and remove unwanted subdirs
$assetsSource      = '..\..\Projects\SS.GameContent\assets'
$licenseFile       = '..\..\..\LICENSE-ASSETS.txt'
$assetsDestination = Join-Path $outputDir "$gameName.$gameVersion.assets\assets"
$subdirsToRemove   = @('bin','obj')

Write-Host "Copying assets to '$assetsDestination'..."
Copy-Item -Path $assetsSource -Destination $assetsDestination -Recurse -Force
Copy-Item -Path $licenseFile -Destination $assetsDestination -Force

foreach ($sub in $subdirsToRemove) {
    $path = Join-Path $assetsDestination $sub
    if (Test-Path $path) {
        Remove-Item $path -Recurse -Force
        Write-Host "Removed subdirectory: $sub"
    } else {
        Write-Host "Subdirectory not found (skipped): $sub"
    }
}

Write-Host "`nBuild pipeline finished successfully."
