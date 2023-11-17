rem Use this batch in order to generate code coverage report for the library
rem Required tools: .net coverage tool and report generator
rem Install them with the following commands
rem dotnet tool install --global dotnet-coverage
rem dotnet tool install --global dotnet-reportgenerator-globaltool

dotnet coverage collect dotnet test --output .\tests\CodeCoverage\CodeCoverage.cobertura.xml --output-format cobertura
reportgenerator -reports:.\tests\CodeCoverage\CodeCoverage.cobertura.xml -targetdir:".\tests\CodeCoverage\Report" -reporttypes:Html -assemblyfilters:+InterAppConnector