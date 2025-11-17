# Write-Host "=== SUBINDO API (.NET) + FRONT (Angular) ==="

# # Caminhos base
# $root = $PSScriptRoot
# $apiPath = Join-Path $root "src/bePatientRegistration.Api"
# $frontPath = Join-Path $root "be-patient-registration"

# Write-Host "Raiz do projeto: $root"
# Write-Host "API path:        $apiPath"
# Write-Host "Front path:      $frontPath"

# # 1) Sobe a API em uma nova janela do PowerShell
# Write-Host "-> Subindo API em uma nova janela..."
# Start-Process powershell -ArgumentList "-NoExit", "-Command", @"
# cd "$apiPath"
# Write-Host '=== API: bePatientRegistration.Api ==='
# dotnet run --project bePatientRegistration.Api.csproj
# "@

# # 2) Sobe o FRONT na janela atual
# Write-Host "-> Subindo FRONT na janela atual..."
# Set-Location -Path $frontPath

# if (-not (Test-Path "node_modules")) {
    # Write-Host "node_modules não encontrado. Instalando dependências com npm install..."
    # npm install
# }

# Write-Host "=== FRONT: be-patient-registration (Angular) ==="
# Write-Host "API deve estar rodando na outra janela."
# Write-Host "Acesse o front em: http://localhost:4200"
# npm start

Write-Host "=== SUBINDO API (.NET) + FRONT (Angular) ==="

# Caminhos base
$root = $PSScriptRoot
$apiPath = Join-Path $root "src/bePatientRegistration.Api"
$frontPath = Join-Path $root "be-patient-registration"

Write-Host "Raiz do projeto: $root"
Write-Host "API path:        $apiPath"
Write-Host "Front path:      $frontPath"

# =============================
# 1) SUBIR A API EM OUTRA JANELA
# =============================
Write-Host "`n-> Subindo API em nova janela PowerShell..."
Start-Process powershell -ArgumentList "-NoExit", "-Command", @"
cd '$apiPath'
Write-Host '=== API: bePatientRegistration.Api ==='
dotnet run --project bePatientRegistration.Api.csproj
"@

# =============================
# 2) ABRIR O SWAGGER AUTOMATICAMENTE
# =============================
Write-Host "Aguardando API iniciar (3 seg)..."
Start-Sleep -Seconds 3

# Porta padrão do Kestrel (ajuste se no seu for outra)
$swaggerUrl = "http://localhost:5234/swagger"
Write-Host "Abrindo Swagger em: $swaggerUrl"
Start-Process $swaggerUrl


# =============================
# 3) SUBIR O FRONT NA JANELA ATUAL
# =============================
Write-Host "`n-> Subindo FRONT Angular..."
Set-Location -Path $frontPath

# Instalar dependências se necessário
if (-not (Test-Path "node_modules")) {
    Write-Host "node_modules não encontrado. Rodando npm install..."
    npm install
}

Write-Host "=== FRONT: be-patient-registration (Angular) ==="
Write-Host "A API está rodando na outra janela."
Write-Host "Abrindo Angular em: http://localhost:4200"
Start-Process "http://localhost:4200"

npm start
