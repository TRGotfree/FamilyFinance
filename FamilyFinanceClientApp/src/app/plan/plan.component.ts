// tslint:disable:align
import { CustomLogger } from './../shared/services/logger.service';
import { Component, OnInit } from '@angular/core';
import { Plan } from './plan.model';
import { PlanService } from './plan.service';
import { MatTableDataSource } from '@angular/material/table';
import { TableColumnAttribute } from '../shared/models/tableColumnAttribute';
import { FormControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  templateUrl: './plan.component.html',
  styleUrls: ['./plan.component.css']
})
export class PlanComponent implements OnInit {
  constructor(private planService: PlanService,
    private snackBar: MatSnackBar, private logger: CustomLogger) {

  }
  searchValueControl = new FormControl('');
  dataSource: MatTableDataSource<Plan>;
  plans: Plan[];
  visibleGridColumns: string[];
  gridColumns: TableColumnAttribute[];
  requiredColumnNames = ['editColumn', 'deleteColumn'];

  ngOnInit(): void {
    this.planService.getPlansMeta().subscribe(tableColumns => {
      if (!tableColumns) {
        this.snackBar.open('Не удалось загрузить метаданные таблицы!', 'OK', { duration: 3000, verticalPosition: 'bottom' });
        return;
      }
    }, error => {
      this.snackBar.open('Не удалось загрузить метаданные таблицы!', 'OK', { duration: 3000, verticalPosition: 'bottom' });
      this.logger.logError(error);
    }, () => {
      const date = new Date();
      this.loadPlans(date.getMonth() + 1, date.getFullYear());
    });
  }

  loadPlans(month: number, year: number) {
    this.planService.getPlans(month, year).subscribe(plans => {
      if (!plans) {
        this.snackBar.open('Не удалось загрузить данные по планам затрат!', 'OK', { duration: 3000, verticalPosition: 'bottom' });
        return;
      }
      this.dataSource = new MatTableDataSource(plans);
    },
      error => {
        this.snackBar.open('Не удалось загрузить данные по планам затрат!', 'OK', { duration: 3000, verticalPosition: 'bottom' });
        this.logger.logError(error);
      }
    );
  }

  searchData(event) {
    const searchValue = (event.target as HTMLInputElement).value;
    if (!searchValue) {
      return;
    }

    this.dataSource.filter = searchValue.trim().toLowerCase();
  }

  resetFilter() {
    this.dataSource.filter = null;
    this.searchValueControl.setValue('');
  }

  editPlan(plan: Plan) {

  }

  deletePlan(plan: Plan) {

  }
}
