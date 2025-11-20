# Este script busca e exclui as pastas 'bin' e 'obj' recursivamente.

# Busca as pastas 'bin' e 'obj' e as armazena na variável $ItemsToRemove
$ItemsToRemove = Get-ChildItem -Path . -Include bin,obj -Recurse -ErrorAction SilentlyContinue

Write-Host "Iniciando a limpeza das pastas 'bin' e 'obj' em sua solução..." -ForegroundColor Yellow

# Verifica se foram encontrados itens para exclusão
if ($ItemsToRemove.Count -gt 0) {
    # Remove os itens, forçando a exclusão (Force) e ignorando erros (ErrorAction SilentlyContinue)
    $ItemsToRemove | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue

    Write-Host "---"
    Write-Host "✅ Limpeza concluída com sucesso! As pastas 'bin' e 'obj' foram excluídas." -ForegroundColor Green
} else {
    Write-Host "---"
    Write-Host "⚠️ Nenhuma pasta 'bin' ou 'obj' encontrada para exclusão na estrutura atual." -ForegroundColor Cyan
}

# Limpa a variável
Remove-Variable ItemsToRemove -ErrorAction SilentlyContinue