// // src/app/pages/health-plans/health-plans-list.component.ts
// import { Component, OnInit } from '@angular/core';
// import { HealthPlanService } from '../../services/health-plan.service';
// import { HealthPlan } from '../../models/health-plan.model';

// @Component({
//   selector: 'app-health-plans-list',
//   standalone: false,
//   templateUrl: './health-plans-list.component.html',
//   styleUrls: ['./health-plans-list.component.scss']
// })
// export class HealthPlansListComponent implements OnInit {
//   healthPlans: HealthPlan[] = [];
//   loading = false;
//   error?: string;

//   constructor(private healthPlanService: HealthPlanService) {}

//   ngOnInit(): void {
//     this.loadPlans();
//   }

//   loadPlans(): void {
//     this.loading = true;
// 	this.error = undefined;
	
//     this.healthPlanService.getAll().subscribe({
//       next: data => {
//         this.healthPlans  = data;
// 		console.log('XXX healthPlanService.getAll() data', data, 'len=', data.length);
//         this.loading = false;
//       },
//       error: err => {
//         console.error(err);
//         this.error = 'Erro ao carregar convênios.';
//         this.loading = false;
//       }
//     });
//   }
// }

import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HealthPlanService } from '../../services/health-plan.service';
import { HealthPlan } from '../../models/health-plan.model';

@Component({
  selector: 'app-health-plans-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './health-plans-list.component.html',
  styleUrls: ['./health-plans-list.component.scss']
})
export class HealthPlansListComponent implements OnInit {

  healthPlans: HealthPlan[] = [];
  loading = false;
  error?: string;

  constructor(private healthPlanService: HealthPlanService) {}

  ngOnInit(): void {
    this.loadPlans();
  }

  loadPlans(): void {
    this.loading = true;
    this.error = undefined;

    this.healthPlanService.getAll().subscribe({
      next: data => {
        console.log("Plans received:", data);
        this.healthPlans = data;
        this.loading = false;
      },
      error: err => {
        console.error(err);
        this.error = 'Erro ao carregar convênios.';
        this.loading = false;
      }
    });
  }
}
