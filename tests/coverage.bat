:: We need to delete previous results
:: More information on rmdir https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/rd
rmdir /s /q CoverageResults

:: We generate Coverage Results with the help of coverlet.msbuild package https://github.com/coverlet-coverage/coverlet
:: for more information review the usage page: https://danielpalme.github.io/ReportGenerator/usage.html
:: CoverageResults/coverage.cobertura.xml file will be used for the HTML report
:: CoverageResults/coverage.json file is used to merge the coverage of each project.
:: Create coverage for CleanTemplate.Application.Test
dotnet test /p:CollectCoverage=true /p:CoverletOutput="../../CoverageResults/" /p:CoverletOutputFormat=\"json,cobertura\" /maxcpucount:1 Core\CleanTemplate.Application.Test\CleanTemplate.Application.Test.csproj
:: Create coverage for CleanTemplate.SharedKernel.Test and merge with previous
dotnet test /p:CollectCoverage=true /p:CoverletOutput="../../CoverageResults/" /p:MergeWith="../../CoverageResults/coverage.json" /p:CoverletOutputFormat=\"json,cobertura\" /maxcpucount:1 Core\CleanTemplate.SharedKernel.Test\CleanTemplate.SharedKernel.Test.csproj
:: Create coverage CleanTemplate.Infrastructure.Integration.Test and merge with previous
dotnet test /p:CollectCoverage=true /p:CoverletOutput="../../CoverageResults/" /p:MergeWith="../../CoverageResults/coverage.json" /p:CoverletOutputFormat=\"json,cobertura\" /maxcpucount:1 Infrastructure\CleanTemplate.Infrastructure.IntegrationTest\CleanTemplate.Infrastructure.IntegrationTest.csproj
:: Create coverage for CleanTemplate.API.Integration.Test and merge with previous
dotnet test /p:CollectCoverage=true /p:CoverletOutput="../../CoverageResults/" /p:MergeWith="../../CoverageResults/coverage.json" /p:CoverletOutputFormat=\"json,cobertura\" /maxcpucount:1 Services\CleanTemplate.API.IntegrationTest\CleanTemplate.API.IntegrationTest.csproj
:: Create coverage for CleanTemplate.GraphQL.Test and merge with previous
dotnet test /p:CollectCoverage=true /p:CoverletOutput="../../CoverageResults/" /p:MergeWith="../../CoverageResults/coverage.json" /p:CoverletOutputFormat=\"json,cobertura\" /maxcpucount:1 Services\CleanTemplate.GraphQL.Test\CleanTemplate.GraphQL.Test.csproj

:: Generate html report with ReportGenerator https://github.com/danielpalme/ReportGenerator
:: First you need to install the tool with
:: dotnet tool install --global dotnet-reportgenerator-globaltool
reportgenerator "-reports:CoverageResults\coverage.cobertura.xml" "-targetdir:CoverageResults\report" -reporttypes:Html
:: Now you can see the results on CoverageResults\report\index.html
