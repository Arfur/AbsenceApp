<#
===============================================================================
   File        : Run-AbsenceApp.ps1
   Author      : Michael
   Version     : 1.3.0
   Created     : 2026-03-24
   Updated     : 2026-05-13
-------------------------------------------------------------------------------
   Purpose     : Parameter-driven build and run script for AbsenceAppV1 and
                 AbsenceAppV2. Ensures deterministic, audit-safe execution
                 using absolute paths and strict validation.
-------------------------------------------------------------------------------
   Notes       :
     - Accepts only 'V1' or 'V2' as valid parameters.
     - Automatically locates the .sln file inside the selected app folder.
     - Cleans and builds the solution before running to ensure runtime consistency.
     - Automatically detects the correct Target Framework (TFM) when multiple
       frameworks are defined in AbsenceApp.Client.csproj.
     - Stops immediately on any error.
-------------------------------------------------------------------------------
   Changes     :
     - 1.0.0  2026-03-24  Initial implementation.
     - 1.1.0  2026-03-24  Updated run logic to execute AbsenceApp.Client.csproj.
     - 1.2.0  2026-03-24  Added automatic framework detection for multi-target
                          projects and updated run command accordingly.
     - 1.3.0  2026-05-13  Added dotnet clean step and set ASPNETCORE_ENVIRONMENT
                          to Development before running.
===============================================================================
#>

param(
    [Parameter(Mandatory = $true)]
    [ValidateSet("V1", "V2")]
    [string]$App
)

# ============================================================================
# Section: Root path and application folder mapping
# ============================================================================
$root = "C:\DevAbsence1"

$paths = @{
    "V1" = Join-Path $root "AbsenceAppV1"
    "V2" = Join-Path $root "AbsenceAppV2"
}

$appPath = $paths[$App]

Write-Host "====================================================="
Write-Host " Building and running AbsenceApp $App"
Write-Host " Path: $appPath"
Write-Host "====================================================="

# ============================================================================
# Section: Validate application folder exists
# ============================================================================
if (-not (Test-Path $appPath)) {
    Write-Error "ERROR: Application folder not found: $appPath"
    exit 1
}

# ============================================================================
# Section: Locate the solution file
# ============================================================================
$solution = Get-ChildItem -Path $appPath -Filter *.sln -Recurse | Select-Object -First 1

if (-not $solution) {
    Write-Error "ERROR: No .sln file found inside $appPath"
    exit 1
}

Write-Host "Solution found: $($solution.FullName)"

# ============================================================================
# Section: Clean the solution (new)
# ============================================================================
Write-Host "Cleaning..."
dotnet clean $solution.FullName

if ($LASTEXITCODE -ne 0) {
    Write-Error "ERROR: Clean failed for AbsenceApp $App"
    exit 1
}

Write-Host "Clean succeeded."

# ============================================================================
# Section: Build the application
# ============================================================================
Write-Host "Building..."
dotnet build $solution.FullName

if ($LASTEXITCODE -ne 0) {
    Write-Error "ERROR: Build failed for AbsenceApp $App"
    exit 1
}

Write-Host "Build succeeded."

# ============================================================================
# Section: Locate the runnable project (AbsenceApp.Client)
# ============================================================================
$project = Get-ChildItem -Path $appPath -Recurse -Filter AbsenceApp.Client.csproj | Select-Object -First 1

if (-not $project) {
    Write-Error "ERROR: Could not find AbsenceApp.Client.csproj inside $appPath"
    exit 1
}

Write-Host "Runnable project found: $($project.FullName)"

# ============================================================================
# Section: Detect target framework (TFM)
# ============================================================================
$xml = [xml](Get-Content $project.FullName)

$tfm = $xml.Project.PropertyGroup.TargetFramework
$tfms = $xml.Project.PropertyGroup.TargetFrameworks

if ($tfm) {
    $framework = $tfm
}
elseif ($tfms) {
    # Choose the first framework (usually the Windows one)
    $framework = $tfms.Split(";")[0]
}
else {
    Write-Error "ERROR: No TargetFramework or TargetFrameworks found in project file."
    exit 1
}

Write-Host "Detected framework: $framework"

# ============================================================================
# Section: Ensure Development environment for local debugging
# ============================================================================
# Set ASPNETCORE_ENVIRONMENT so appsettings.Development.json is used.
$env:ASPNETCORE_ENVIRONMENT = "Development"
Write-Host "ASPNETCORE_ENVIRONMENT set to: $env:ASPNETCORE_ENVIRONMENT"

# ============================================================================
# Section: Run the application
# ============================================================================
Write-Host "Running AbsenceApp $App..."
dotnet run --no-build --project $project.FullName --framework $framework
