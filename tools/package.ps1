# Builds Geocrest NuGet packages from .csproj sources. 
# Most package properties are maintained in .nuspec files within each project folder.
# Note that package dependencies are discovered from each project's packages.xml.
#
# Requirements: 
# 1. nuget.exe 4.3+(CLI) must be available on $env:Path.
# 2. Caller must update $Version to desired package version.


$Version = "3.0.0"
$Verbosity = "detailed"
$OutputFolder = "output"

# Define Nuget executable.
$exe = "nuget.exe"

# Save current directory and move current to repo root.
Push-Location -Path $PSScriptRoot
Set-Location (Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)
Set-Location ..\
$RootFolder = Get-Location

# Create output folder at root and set to current directory for nuget's OutputDirectory parameter.
New-Item $OutputFolder -Type Directory -ErrorAction Ignore
Set-Location $OutputFolder
Write-Output ("Package output folder: {0}\{1}" -f $RootFolder, $OutputFolder)

# Set package source used throughout solution.
&$exe config -Set repositoryPath=("{0}\packages" -f $RootFolder)

# Pack.
&$exe pack ("{0}\src\Geocrest.Data.Sources\Geocrest.Data.Sources.csproj" -f $RootFolder) -Version $Version -Properties "Configuration=Release" -Symbols -Verbosity $verbosity
&$exe pack ("{0}\src\Geocrest.Data.Sources.Gis\Geocrest.Data.Sources.Gis.csproj" -f $RootFolder) -Version $Version -Properties "Configuration=Release" -Symbols -Verbosity $verbosity
&$exe pack ("{0}\src\Models\4.5\Geocrest.Model\Geocrest.Model.csproj" -f $RootFolder) -Version $Version -Properties "Configuration=Release" -Symbols -Verbosity $verbosity
&$exe pack ("{0}\src\Geocrest.Web.Infrastructure\Geocrest.Web.Infrastructure.csproj" -f $RootFolder) -Version $Version -Properties "Configuration=Release" -Symbols -Verbosity $verbosity
&$exe pack ("{0}\src\Geocrest.Web.Mvc\Geocrest.Web.Mvc.csproj" -f $RootFolder) -Version $Version -Properties "Configuration=Release" -Symbols -Verbosity $verbosity

# Reset current directory.
Pop-Location
