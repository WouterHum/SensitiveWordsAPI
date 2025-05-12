# Define the paths
$projectPath = "C:\Users\woute\source\repos\SensitiveWordsAPI\"  # Replace with the actual path to your solution
$testProjectName = "SensitiveWordsAPI.Tests"  # Replace with your test project name
$testResultsPath = "$projectPath\TestResults"
$coverageReportPath = "$projectPath\CoverageReport"

# Navigate to the project directory
Set-Location -Path $projectPath

# Ensure the test project is restored
Write-Host "Restoring the test project..."
dotnet restore "$testProjectName"

# Run the tests and collect code coverage
Write-Host "Running tests and collecting coverage..."
dotnet test "$testProjectName" --collect:"XPlat Code Coverage"

# Check if the test results folder exists
if (Test-Path -Path $testResultsPath) {
    # Locate the latest coverage report in .cobertura.xml format
    $coverageXmlFile = Get-ChildItem -Path $testResultsPath -Recurse -Filter "coverage.cobertura.xml" | Sort-Object LastWriteTime -Descending | Select-Object -First 1
    
    if ($coverageXmlFile) {
        Write-Host "Found coverage report: $($coverageXmlFile.FullName)"

        # Install ReportGenerator tool if not already installed
        Write-Host "Installing ReportGenerator tool..."
        dotnet tool install -g dotnet-reportgenerator-globaltool

        # Generate the HTML report
        Write-Host "Generating the HTML report..."
        reportgenerator -reports:$coverageXmlFile.FullName -targetdir:$coverageReportPath -reporttypes:Html

        # Open the generated HTML report
        Start-Process "$coverageReportPath\index.html"
    } else {
        Write-Host "No coverage report found."
    }
} else {
    Write-Host "TestResults folder not found. Make sure tests are running successfully."
}

Write-Host "Script completed."
