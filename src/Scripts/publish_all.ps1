# --------------------------------------------- 
# Stardust Sandbox Build Pipeline (en-US)
# ---------------------------------------------

# Clear console
Clear-Host

# Configuration
$gameName = 'StardustSandbox'
$gameVersion = 'v2.3.0.0'
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
