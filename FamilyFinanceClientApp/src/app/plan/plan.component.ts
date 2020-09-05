import { EditPlanComponent } from './dialogs/editPlan.component';
// tslint:disable:align
import { CustomLogger } from './../shared/services/logger.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Plan } from './plan.model';
import { PlanService } from './plan.service';
import { MatTableDataSource } from '@angular/material/table';
import { TableColumnAttribute } from '../shared/models/tableColumnAttribute';
import { FormControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationComponent } from '../shared/dialogs/confirmation.component';

@Component({
  templateUrl: './plan.component.html',
  styleUrls: ['./plan.component.css']
})
export class PlanComponent implements OnInit {

  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(private planService: PlanService,
    private snackBar: MatSnackBar, private logger: CustomLogger, private dialogRef: MatDialog) {
  }
  months = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
  years = [];
  selectedMonth = new Date().getMonth() + 1;
  selectedYear = new Date().getFullYear();
  searchValueControl = new FormControl('');
  monthFormControl = new FormControl(new Date().getMonth() + 1);
  yearFormControl = new FormControl(new Date().getFullYear());
  dataSource = new MatTableDataSource<Plan>();
  plans: Plan[];
  visibleGridColumns: string[];
  gridColumns: TableColumnAttribute[];
  requiredColumnNames = ['editColumn'];
  isLoading = true;

  ngOnInit(): void {
    this.years = this.getYears();
    this.isLoading = true;
    this.planService.getPlansMeta().subscribe(tableColumns => {
      if (!tableColumns) {
        this.snackBar.open('Не удалось загрузить метаданные таблицы!', 'OK', { duration: 3000, verticalPosition: 'bottom' });
        return;
      }

      this.gridColumns = tableColumns;
      this.visibleGridColumns = tableColumns.map(d => d.propertyName);
      for (const requiredColumn of this.requiredColumnNames) {
        this.visibleGridColumns.push(requiredColumn);
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
    this.isLoading = true;
    this.planService.getPlans(month, year).subscribe(plans => {
      if (!plans) {
        this.snackBar.open('Не удалось загрузить данные по планам затрат!', 'OK', { duration: 3000, verticalPosition: 'bottom' });
        return;
      }
      this.plans = plans;
      this.dataSource = new MatTableDataSource(plans);
      this.dataSource.sort = this.sort;
    },
      error => {
        this.snackBar.open('Не удалось загрузить данные по планам затрат!', 'OK', { duration: 3000, verticalPosition: 'bottom' });
        this.logger.logError(error);
      },
      () => this.isLoading = !this.isLoading
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

    if (!plan) {
      return;
    }

    const month = this.monthFormControl.value as number;
    const year = this.yearFormControl.value as number;

    if (isNaN(year) || isNaN(year)) {
      this.snackBar.open('Выберите месяц и год для ввода суммы!', 'OK', { duration: 3000 });
      return;
    }

    plan.month = month;
    plan.year = year;

    const editPlanDialog = this.dialogRef.open(EditPlanComponent, { width: '320px', height: '220px', data: plan });
    editPlanDialog.afterClosed().subscribe(editedPlan => {
      if (!editedPlan) {
        return;
      }

      if (editedPlan && editedPlan.id > 0) {
        this.planService.updatePlan(editedPlan).subscribe(updatedPlan => {
          if (!updatedPlan) {
            this.snackBar.open('Не удалось сохранить данные по расходу!', 'OK', { duration: 3000, verticalPosition: 'top' });
            return;
          }

          this.plans[this.plans.findIndex(p => p.id === editedPlan.id)] = updatedPlan;
          this.dataSource.data = this.plans;

        }, error => {
          this.logger.logError(error);
          this.snackBar.open('Не удалось сохранить данные по расходу!', 'OK', { duration: 3000, verticalPosition: 'top' });
        });
      } else {
        this.planService.addPlan(editedPlan).subscribe(newPlan => {
          if (!newPlan) {
            this.snackBar.open('Не удалось сохранить данные по расходу!', 'OK', { duration: 3000, verticalPosition: 'top' });
            return;
          }

          newPlan.maxFactAmount = editedPlan.maxFactAmount;

          this.plans[this.plans.findIndex(p => p.categoryId === newPlan.categoryId)] = newPlan;
          this.dataSource.data = this.plans;
        }, error => {
          this.logger.logError(error);
          this.snackBar.open('Не удалось сохранить данные по расходу!', 'OK', { duration: 3000, verticalPosition: 'top' });
        });
      }

    }, error => this.logger.logError(error), () => this.resetFilter());
  }

  deletePlan(plan: Plan) {
    if (!plan || plan.id <= 0) {
      this.snackBar.open('Нельзя удалять несуществующий расход!', 'OK', { duration: 3000, verticalPosition: 'top', horizontalPosition: 'center' });
      return;
    }
    const confirmationDialog = this.dialogRef.open(ConfirmationComponent,
      { height: '200px', width: '370px', data: { caption: 'Внимание', message: 'Вы уверены что хотите удалить плановое значение?' } });
    confirmationDialog.afterClosed().subscribe(result => {
      if (!result) {
        return;
      }
      this.planService.deletePlan(plan.id).subscribe(() => {
        plan.id = 0; 
        plan.amount = 0;
        plan.amountToDisplay = "0";

        this.plans[this.plans.findIndex(p => p.id === plan.id)] = plan;
        this.plans = this.plans.filter(p => p.id !== plan.id);
        this.dataSource.data = this.plans;
      });
    });
  }

  getTotalPlans() {
    let total = 0;
    this.plans?.map(v => total += v.amount)

    return total;
  }

  private getYears(): number[] {
    const years = [];
    for (let index = 2020; index < 2051; index++) {
      years.push(index);
    }
    return years;
  }
}
