# ---------------------------------------------
#
#  Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
# 
#  This program is free software: you can redistribute it and/or modify
#  it under the terms of the GNU General Public License as published by
#  the Free Software Foundation, either version 3 of the License, or
#  (at your option) any later version.
# 
#  This program is distributed in the hope that it will be useful,
#  but WITHOUT ANY WARRANTY; without even the implied warranty of
#  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
#  GNU General Public License for more details.
# 
#  You should have received a copy of the GNU General Public License
#  along with this program. If not, see <https://www.gnu.org/licenses/>.
#
# ---------------------------------------------

# --------------------------------------------- 
# Stardust Sandbox Build Pipeline (en-US)
# ---------------------------------------------

# Clear console
Clear-Host

# Configuration
$gameName = 'StardustSandbox'
$outputDir = '..\Publish'

# Project definitions
$projects = @(
    @{ Name='Windows'; Path='..\Desktop\SS.Desktop.Windows.csproj' },
    @{ Name='Linux'; Path='..\Desktop\SS.Desktop.Linux.csproj' }
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

    $publishDir = Join-Path $outputDir "$gameName.$projectName"
    dotnet publish $projectPath -c Release --output $publishDir
    
	if ($LASTEXITCODE -ne 0) {
        return
    }
}

# Execute publishes
foreach ($proj in $projects) {
    Publish-Project -projectName $proj.Name -projectPath $proj.Path
}
