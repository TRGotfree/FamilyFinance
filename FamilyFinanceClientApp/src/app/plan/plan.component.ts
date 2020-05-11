import { Component, OnInit } from '@angular/core';
import { Plan } from './plan.model';
import { PlanService } from './plan.service';

@Component({
  templateUrl: './plan.component.html',
  styleUrls: ['./plan.component.css']
})
export class PlanComponent implements OnInit {
  constructor(private planService: PlanService) { }

  ngOnInit(): void { }
}
