import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Income } from './income.model';
import { MatTableDataSource } from '@angular/material/table';
import { TableColumnAttribute } from '../shared/models/tableColumnAttribute';
import { IncomesService } from './incomes.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PayType } from '../dictionaries/paytype/paytype.model';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { PayTypeService } from '../dictionaries/paytype/paytType.service';
import { DateTimeBuilder } from '../shared/services/dateTimeBuilder.service';
import { MomentInputObject, Moment, MomentInput, MomentObjectOutput } from 'moment';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import * as moment from 'moment';

@Component({
    templateUrl: 'incomes.component.html',
    styleUrls: ['incomes.component.css']
})

export class IncomesComponent implements OnInit {

    isLoading = false;
    selectedIncome: Income = { id: 0, amount: 0, comment: '', date: '', payTypeId: 0, payType: '' };
    incomeFormHeader = 'Новый доход'
    dataSource = new MatTableDataSource([]);
    incomes: Income[];
    visibleGridColumns: string[];
    gridColumns: TableColumnAttribute[];
    requiredColumnNames = ['editColumn', 'deleteColumn'];

    payTypes: PayType[] = [];
    filteredIncomeTypes: Observable<PayType[]>;

    incomesBeginDateControl: FormControl;
    incomesEndDateControl: FormControl;

    incomeFormGroup: FormGroup;
    incomeAmountControl: FormControl;
    commentControl: FormControl;
    incomeTypeControl: FormControl;
    incomeDateControl: FormControl;

    constructor(private incomesService: IncomesService, private payTypesService: PayTypeService,
        private dateTimeBuilder: DateTimeBuilder, private snackBar: MatSnackBar) {

        this.incomesBeginDateControl = new FormControl(this.getPreviousMonthbeginDate());
        this.incomesEndDateControl = new FormControl(moment());

        this.incomeAmountControl = new FormControl(0, [Validators.required, Validators.min(1)]);
        this.commentControl = new FormControl('', [Validators.maxLength(2000)]);
        this.incomeTypeControl = new FormControl(null, [Validators.required]);
        this.incomeDateControl = new FormControl(moment(), [Validators.required]);

        this.incomeFormGroup = new FormGroup({
            incomeAmount: this.incomeAmountControl,
            incomeType: this.incomeTypeControl,
            incomeDate: this.incomeDateControl,
            comment: this.commentControl
        });

        this.filteredIncomeTypes = this.incomeTypeControl.valueChanges
            .pipe(startWith(''),
                map(value => value ? value : ''),
                map(value => (typeof value === 'string') ? value : value.sysName),
                map(name => name ? this.filterIncome(name) : this.payTypes.slice()))
    }

    ngOnInit() {
        this.incomesService.getIncomesMeta().subscribe(metaData => {
            if (!metaData || metaData.length === 0) {
                this.snackBar.open('не удалось загрузить метаданные для таблицы доходов!', 'OK', { duration: 3000 })
                return;
            }

            this.gridColumns = metaData;
            this.visibleGridColumns = metaData.map(d => d.propertyName);
            for (const requiredColumn of this.requiredColumnNames) {
                this.visibleGridColumns.push(requiredColumn);
            }

            this.loadData();
        }, error => {
            this.snackBar.open('не удалось загрузить метаданные для таблицы доходов! Произошла ошибка!', 'OK', { duration: 3000 })
        });
    }

    loadData() {

        if (!this.incomesBeginDateControl.value || !this.incomesEndDateControl.value) {
            return;
        }

        this.isLoading = true;

        this.incomesService.getIncomes(this.incomesBeginDateControl.value.toDate(), this.incomesEndDateControl.value.toDate())
            .subscribe(incomesData => {
                if (!incomesData) {
                    this.snackBar.open('Не удалось загрузить данные по доходам!', 'OK', { duration: 3000 });
                    return;
                }

                this.incomes = incomesData;
                this.dataSource = new MatTableDataSource(this.incomes);
            }, error => {
                this.snackBar.open('Произошла ошибка! Не удалось загрузить данные по доходам!', 'OK', { duration: 3000 });
            }, () => {
                this.isLoading = false;
            });

        this.payTypesService.getPayTypes().subscribe(payTypesData => {
            if (!payTypesData) {
                this.snackBar.open('Не удалось загрузить данные по типам доходов!', 'OK', { duration: 3000 })
                return;
            }

            this.payTypes = payTypesData;
        }, error => {
            this.snackBar.open('Не удалось загрузить данные по типам доходов! Произошла ошибка!', 'OK', { duration: 3000 })
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

        if (this.selectedIncome.id <= 0) {

            const newIncome: Income = {
                id: 0,
                amount: this.incomeAmountControl.value,
                date: this.dateTimeBuilder.getFormattedDate(this.incomeDateControl.value.toDate(), '-'),
                payTypeId: this.incomeTypeControl.value.id,
                payType: this.incomeTypeControl.value.name,
                comment: this.commentControl.value
            }

            this.incomesService.addIncome(newIncome).subscribe(addedIncome => {
                if (!addedIncome) {
                    this.snackBar.open('Не удалось сохранить данные по доходу!', 'OK', { duration: 3000 });
                    return;
                }

                this.incomes.push(addedIncome);
                this.dataSource = new MatTableDataSource(this.incomes);

            }, error => { 
                this.snackBar.open('Не удалось сохранить данные по доходу!', 'OK', { duration: 3000 });
            });
        } else {

            const editedIncome: Income = {
                id: this.selectedIncome.id,
                amount: this.incomeAmountControl.value,
                date: this.dateTimeBuilder.getFormattedDate(this.incomeDateControl.value.toDate(), '-'),
                payTypeId: this.incomeTypeControl.value.id,
                payType: this.incomeTypeControl.value.name,
                comment: this.commentControl.value
            }

            this.incomesService.editIncome(editedIncome).subscribe(editedIncome => {
                if (!editedIncome) {
                    this.snackBar.open('Не удалось сохранить данные по доходу!', 'OK', { duration: 3000 });
                    return;
                }

                let incomeToUpdate = this.incomes.find(i => i.id === editedIncome.id);
                incomeToUpdate = editedIncome;

            });
        }

    }

    editIncome(income: Income) {

    }

    deleteIncome(income: Income) {

    }

    private filterIncome(value: string) {
        const filteredValue = value.toLowerCase();
        return this.payTypes.filter(pt => pt.name.toLowerCase().includes(filteredValue));
    }

    displayIncomeType(payType: PayType) {
        return payType && payType.name ? payType.name : '';
    }

    private getPreviousMonthbeginDate() {
        // let currentDate = moment();
        // let previousMonth = currentDate.month();
        // let year = currentDate.year();
        return moment().add(-1, 'M');
    }
}