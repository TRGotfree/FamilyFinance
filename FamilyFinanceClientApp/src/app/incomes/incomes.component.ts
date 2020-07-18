import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Income } from './income.model';
import { MatTableDataSource } from '@angular/material/table';
import { TableColumnAttribute } from '../shared/models/tableColumnAttribute';
import { IncomesService } from './incomes.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
    templateUrl: 'incomes.component.html',
    styleUrls: ['incomes.component.css']
})

export class IncomesComponent implements OnInit {

    isLoading = false;
    dataSource = new MatTableDataSource([]);
    incomes: Income[];
    visibleGridColumns: string[];
    gridColumns: TableColumnAttribute[];
    requiredColumnNames = ['editColumn', 'deleteColumn'];

    incomesBeginDateControl: FormControl;
    incomesEndDateControl: FormControl;

    incomeFormGroup: FormGroup;
    incomeAmountControl: FormControl;
    commentControl: FormControl;
    incomeTypeControl: FormControl;
    incomeDateControl: FormControl;

    constructor(private incomesService: IncomesService, private snackBar: MatSnackBar) {

        this.incomesBeginDateControl = new FormControl(this.getPreviousMonthbeginDate());
        this.incomesEndDateControl = new FormControl(new Date());

        this.incomeAmountControl = new FormControl(0, [Validators.required, Validators.min(1)]);
        this.commentControl = new FormControl('');
        this.incomeTypeControl = new FormControl(null, [Validators.required]);
        this.incomeDateControl = new FormControl(new Date(), [Validators.required]);

        this.incomeFormGroup = new FormGroup({
            incomeAmount: this.incomeAmountControl,
            incomeType: this.incomeTypeControl,
            incomeDate: this.incomeDateControl,
            comment: this.commentControl
        });
    }

    ngOnInit() {
        this.loadData();
    }

    loadData() {

        if (!this.incomesBeginDateControl.value || !this.incomesEndDateControl.value) {
            return;
        }

        this.isLoading = true;

        this.incomesService.getIncomes(this.incomesBeginDateControl.value, this.incomesEndDateControl.value)
            .subscribe(incomesData => {
                if (!incomesData) {
                    this.snackBar.open('Не удалось загрузить данные по доходам!', 'OK', { duration: 3000 });
                    return;
                }

                this.incomes = incomesData;

            }, error => {
                this.snackBar.open('Произошла ошибка! Не удалось загрузить данные по доходам!', 'OK', { duration: 3000 });
            }, () => { 
                this.isLoading = false;
            });
    }

    incomeTypeChoosed(event) {

    }

    clearIncomeTypeControl() {
        this.incomeTypeControl.setValue(null);
    }

    saveIncome() {
        if (this.incomeFormGroup.invalid) {
            return;
        }


    }

    editIncome(income: Income) {

    }

    deleteIncome(income: Income) {

    }

    private getPreviousMonthbeginDate(): Date {
        let currentDate = new Date();
        let previousMonth = currentDate.getMonth() - 1 < 0 ? 12 : currentDate.getMonth() - 1;
        let year = currentDate.getFullYear();
        return new Date(year, previousMonth, 1);
    }
}