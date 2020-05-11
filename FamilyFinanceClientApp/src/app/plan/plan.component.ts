import { CustomLogger } from './../shared/services/logger.service';
import { Component, OnInit } from '@angular/core';
import { Plan } from './plan.model';
import { PlanService } from './plan.service';

@Component({
  templateUrl: './plan.component.html',
  styleUrls: ['./plan.component.css']
})
export class PlanComponent implements OnInit {
  constructor(private planService: PlanService, private logger: CustomLogger) { }

  ngOnInit(): void {
    this.planService.getPlansMeta().subscribe(tableColumns => {

    }, error => {
      this.logger.logError(error);
    }, () => {
      const date = new Date();
      this.loadPlans(date.getMonth() + 1, date.getFullYear());
    });
  }

  loadPlans(month: number, year: number) {
    this.planService.getPlans(month, year).subscribe(plans => {

    },
      error => {
        this.logger.logError(error);
      }
    );
  }
}
