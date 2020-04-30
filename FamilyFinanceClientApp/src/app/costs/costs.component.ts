// tslint:disable:align
import { CustomLogger } from './../common/logger.service';
import { CostsTableColumn } from './costsTableColumns.model';
import { Component, OnInit, ViewChild } from '@angular/core';
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
export class CostsComponent implements OnInit {

  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(private costsService: CostsService, private logger: CustomLogger,
    private snackBar: MatSnackBar, private dialog: MatDialog) {

  }

  costsDateControl = new FormControl(new Date());
  dataSource = new MatTableDataSource([]);
  selectedDate: Date;
  costs: Cost[];
  visibleGridColumns: string[];
  gridColumns: CostsTableColumn[];
  searchValue = new FormControl('');
  requiredColumnNames = ['editColumn'];

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
      this.costsService.getCosts(new Date())
        .subscribe(data => {
          this.costs = data;
          this.dataSource = new MatTableDataSource(this.costs);
          this.dataSource.sort = this.sort;
        }, error => {
          this.logger.logError(error);
          this.snackBar.open('Не удалось загрузить данные по расходам!', 'OK', { duration: 3000 });
        });
    });

  }

  searchData(event: Event) {
    const searchValue = (event.target as HTMLInputElement).value;
    if (!searchValue) { return; }

    this.dataSource.filter = searchValue.trim().toLowerCase();
  }

  editCost(cost: Cost) {
    const costDate = new Date(cost.date);
    const costControlDate = this.costsDateControl.value as Date;

    if (costDate.getFullYear() === 1 && !costControlDate) {
      this.snackBar.open('Выберите дату расхода!', 'OK', { duration: 3000 });
      return;
    }

    cost.date = costDate.getFullYear() === 1 ? costControlDate.toISOString().substring(0, 10) : cost.date;

    const newCostDialog = this.dialog.open(NewCostComponent, { width: '500px', height: '550px', data: cost });
    newCostDialog.afterClosed().subscribe(editedCost => {
      // this.costs.find(c => c.categoryId === editedCost.categoryId);
    }, error => this.logger.logError(error));


  }

}
