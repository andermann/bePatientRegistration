#!/usr/bin/env bash
set -e

# Layout + página inicial
ng g c layout/navbar --standalone --skip-tests
ng g c pages/home --standalone --skip-tests

# Pacientes
ng g s features/patients/services/patients --flat --skip-tests
ng g c features/patients/pages/patients-list --standalone --skip-tests
ng g c features/patients/pages/patient-form --standalone --skip-tests

# Convênios
ng g s features/health-plans/services/health-plans --flat --skip-tests
ng g c features/health-plans/pages/health-plans-list --standalone --skip-tests
ng g c features/health-plans/pages/health-plan-form --standalone --skip-tests

# Models e validators
mkdir -p src/app/core/models
touch src/app/core/models/patient.model.ts
touch src/app/core/models/health-plan.model.ts

mkdir -p src/app/core/validators
touch src/app/core/validators/custom-validators.ts

echo "Estrutura standalone criada."
