import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { HealthPlansService } from '../../services/health-plans.service';
import { HealthPlan } from '../../../../core/models/health-plan.model';

@Component({
  selector: 'app-health-plans-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './health-plans-list.component.html'
})
export class HealthPlansListComponent implements OnInit {
  healthPlans: HealthPlan[] = [];
  loading = false;
  error?: string;

  constructor(
    private healthPlansService: HealthPlansService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.error = undefined;

    this.healthPlansService.getAll().subscribe({
      next: (data) => {
        console.log('health plans data', data);
        this.healthPlans = data ?? [];
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Erro ao carregar convênios', err);
        this.error = 'Erro ao carregar convênios.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  newPlan(): void {
    this.router.navigate(['/convenios/novo']);
  }

  editPlan(plan: HealthPlan): void {
    this.router.navigate(['/convenios', plan.id]);
  }

  deletePlan(plan: HealthPlan): void {
    if (!confirm(`Deseja inativar o convênio "${plan.name}"?`)) {
      return;
    }

    this.healthPlansService.delete(plan.id).subscribe({
      next: () => this.load(),
      error: () => alert('Erro ao inativar convênio.')
    });
  }
}
