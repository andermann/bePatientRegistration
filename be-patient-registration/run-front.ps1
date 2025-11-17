Write-Host "=== FRONTEND: be-patient-registration ==="

# Garante que o script está rodando na pasta do projeto Angular
Set-Location -Path $PSScriptRoot

# Instala as dependências só se a pasta node_modules não existir
if (-not (Test-Path "node_modules")) {
    Write-Host "node_modules não encontrado. Instalando dependências com npm install..."
    npm install
}

Write-Host "Subindo Angular dev server com 'npm start'..."
Write-Host "Abra depois: http://localhost:4200"
# npm start

npm run build
# ou
npm start
# ou
# ng serve --open
