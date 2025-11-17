Write-Host "=== BUILDANDO/PUBLISH da API bePatientRegistration ==="
dotnet publish "./bePatientRegistration.Api.csproj" -c Release -o "./publish"

Write-Host "=== DEFININDO CONNECTION STRING (SQL Server) ==="

# ⚠️ COPIE A MESMA connection string que você usa no appsettings.Development.json
# e cole exatamente no lugar de SUA_CONNECTION_STRING_AQUI

$env:ConnectionStrings__DefaultConnection = "Server=localhost,1400;Database=BePatientRegistrationDb;User Id=sa;Password=Your_strong_Password123;Encrypt=False;TrustServerCertificate=True;MultipleActiveResultSets=True;"

Write-Host "=== SUBINDO A API (Release) ==="
Set-Location "./publish"
dotnet bePatientRegistration.Api.dll
