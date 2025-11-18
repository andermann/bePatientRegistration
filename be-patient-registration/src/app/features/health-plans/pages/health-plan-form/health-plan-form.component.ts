import { Component, OnInit, inject } from '@angular/core';
import { NgIf } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { HealthPlansService } from '../../services/health-plans.service';
import {
  HealthPlan,
  CreateHealthPlanRequest,
  UpdateHealthPlanRequest
} from '../../../../core/models/health-plan.model';

@Component({
  selector: 'app-health-plan-form',
  standalone: true,
  imports: [NgIf, ReactiveFormsModule],
  templateUrl: './health-plan-form.component.html'
})
export class HealthPlanFormComponent implements OnInit {
  // FormBuilder injetado como propriedade, antes do form
  private fb = inject(FormBuilder);

  form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    isActive: [true]
  });

  loading = false;
  isEditMode = false;
  id?: string;
  error?: string;

  constructor(
    private healthPlansService: HealthPlansService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id') || undefined;
    this.isEditMode = !!this.id;

    if (this.isEditMode && this.id) {
      this.loadPlan(this.id);
    }
  }

  get f() {
    return this.form.controls;
  }

  private loadPlan(id: string): void {
    this.loading = true;
    this.healthPlansService.getById(id).subscribe({
      next: (plan: HealthPlan) => {
        this.form.patchValue({
          name: plan.name,
          isActive: plan.isActive
        });
        this.loading = false;
      },
      error: () => {
        this.error = 'Convênio não encontrado.';
        this.loading = false;
      }
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.getRawValue();

    const baseRequest: CreateHealthPlanRequest = {
      name: raw.name!
    };

    this.loading = true;

    if (this.isEditMode && this.id) {
      const request: UpdateHealthPlanRequest = {
        ...baseRequest,
        isActive: raw.isActive!
      };

      this.healthPlansService.update(this.id, request).subscribe({
        next: () => this.router.navigate(['/convenios']),
        error: () => {
          this.error = 'Erro ao salvar convênio.';
          this.loading = false;
        }
      });
    } else {
      this.healthPlansService.create(baseRequest).subscribe({
        next: () => this.router.navigate(['/convenios']),
        error: () => {
          this.error = 'Erro ao salvar convênio.';
          this.loading = false;
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/convenios']);
  }
}
