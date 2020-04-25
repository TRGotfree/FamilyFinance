import { CostsTableColumns } from './costsTableColumns.model';
import { Component, OnInit, ViewChild } from '@angular/core';
import { CostsService } from './costs.service';
import { Cost } from './cost.model';
import { MatPaginator, MatPaginatorIntl } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';

import { from } from 'rxjs';
@Component({
  templateUrl: './costs.component.html',
  styleUrls: ['./costs.component.css']
})
export class CostsComponent implements OnInit {

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginatorIntl, { static: false }) matPaginatorIntl: MatPaginatorIntl;

  constructor(private costsService: CostsService,
              private snackBar: MatSnackBar, private dialog: MatDialog) {

  }

  dataSource = new MatTableDataSource([]);
  selectedDate: Date;
  costs: Cost[];
  visibleGridColumns: string[];
  gridColumns: CostsTableColumns[];

  ngOnInit(): void {
    this.costsService.getCosts(new Date())
      .subscribe(data => {
        this.dataSource = new MatTableDataSource(data);
      }, error => {
        this.snackBar.open('Не удалось загрузить данные', 'OK', { duration: 3000 });
      }, () => {

      });
  }

}
