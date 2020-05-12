import { ConfirmationComponent } from './../shared/dialogs/confirmation.component';
// tslint:disable:align
import { CustomLogger } from './../shared/services/logger.service';
import { TableColumnAttribute } from '../shared/models/tableColumnAttribute';
import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { CostsService } from './costs.service';
import { Cost } from './cost.model';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { FormControl } from '@angular/forms';
import { NewCostComponent } from './dialogs/newCost.component';

@Component({
  templateUrl: './costs.component.html',
  styleUrls: ['./costs.component.css']
})
export class CostsComponent implements OnInit, AfterViewInit {

  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(private costsService: CostsService, private logger: CustomLogger,
    private snackBar: MatSnackBar, private dialogRef: MatDialog) {
  }

  costsDateControl = new FormControl(new Date());
  dataSource = new MatTableDataSource([]);
  selectedDate: Date;
  costs: Cost[];
  visibleGridColumns: string[];
  gridColumns: TableColumnAttribute[];
  searchValueControl = new FormControl('');
  requiredColumnNames = ['editColumn', 'deleteColumn'];

  ngOnInit(): void {

    this.costsService.getCostsMeta().subscribe(data => {
      this.gridColumns = data;
      this.visibleGridColumns = data.map(d => d.propertyName);
      for (const requiredColumn of this.requiredColumnNames) {
        this.visibleGridColumns.push(requiredColumn);
      }
    }, error => {
      this.logger.logError(error);
      this.snackBar.open('Не удалось загрузить метаданные для формирования таблицы расходов!', 'OK', { duration: 3000 });
    }, () => {
      this.loadCostsOnDate(new Date());
    });

  }

  ngAfterViewInit() {
    this.costsDateControl.valueChanges.subscribe(dateControl => {
      const date = dateControl.format('YYYY-MM-DD HH:mm:ss');
      this.loadCostsOnDate(new Date(date));
    }, error => this.logger.logError(error));
  }

  loadCostsOnDate(date: Date) {
    this.costsService.getCosts(date)
      .subscribe(data => {
        this.costs = data;
        this.dataSource = new MatTableDataSource(this.costs);
        this.dataSource.sort = this.sort;
      }, error => {
        this.logger.logError(error);
        this.snackBar.open('Не удалось загрузить данные по расходам!', 'OK', { duration: 3000 });
      });
  }

  searchData(event: Event) {
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

  editCost(cost: Cost) {
    const costDate = new Date(cost.date);
    const costControlDate = this.costsDateControl.value as Date;

    if (costDate.getFullYear() === 1 && !costControlDate) {
      this.snackBar.open('Выберите дату расхода!', 'OK', { duration: 3000 });
      return;
    }

    cost.date = costDate.getFullYear() === 1 ? costControlDate.toISOString().substring(0, 10) : cost.date;

    const newCostDialog = this.dialogRef.open(NewCostComponent, { width: '500px', height: '550px', data: cost });
    newCostDialog.afterClosed().subscribe(editedCost => {
      if (!editedCost) {
        return;
      }

      this.resetFilter();
    }, error => this.logger.logError(error));
  }

  deleteCost(cost: Cost) {
    if (!cost || cost.id <= 0) {
      this.snackBar.open('Нельзя удалять несуществующий расход!', 'OK', { duration: 3000, verticalPosition: 'top', horizontalPosition: 'center' });
      return;
    }
    const confirmationDialog = this.dialogRef.open(ConfirmationComponent,
      { height: '180px', width: '350px', data: { caption: 'Внимание', message: 'Вы уверены что хотите удалить расход?' } });
    confirmationDialog.afterClosed().subscribe(result => {
      if (!result) {
        return;
      }
      this.costsService.deleteCost(cost).subscribe(() => {
        cost.id = 0;
        cost.amount = 0;
        cost.comment = '';
        cost.count = '';
        cost.store = '';
        cost.storeId = 0;
        cost.payType = '';
        cost.payTypeId = 0;
        cost.date = new Date(1, 1, 1).toString();
      });
    });
  }

}
