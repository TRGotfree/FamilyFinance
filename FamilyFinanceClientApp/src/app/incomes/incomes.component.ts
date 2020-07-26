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
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationComponent } from '../shared/dialogs/confirmation.component';

@Component({
    templateUrl: 'incomes.component.html',
    styleUrls: ['incomes.component.css']
})

export class IncomesComponent implements OnInit {

    isLoading = false;
    selectedIncome: Income = {
        id: 0,
        amount: 0, comment: '',
        date: '', payTypeId: 0,
        payType: '', amountToDisplay: '0',
        shortComment: ''
    };
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
        private dateTimeBuilder: DateTimeBuilder, private snackBar: MatSnackBar, private dialogRef: MatDialog) {

        this.incomesBeginDateControl = new FormControl(this.getPreviousMonthbeginDate());
        this.incomesEndDateControl = new FormControl(moment());

        this.setControlsWithDefaultsValues();

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
            .pipe(map(i => {
                i.forEach(d => d.date = d.date.substring(0, 10));
                return i;
            }))
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

        if (!this.selectedIncome || this.selectedIncome.id <= 0) {

            const newIncome: Income = {
                id: 0,
                amount: this.incomeAmountControl.value,
                date: this.dateTimeBuilder.getFormattedDate(this.incomeDateControl.value.toDate(), '-'),
                payTypeId: this.incomeTypeControl.value.id,
                payType: this.incomeTypeControl.value.name,
                comment: this.commentControl.value,
                amountToDisplay: null,
                shortComment: null
            }

            this.incomesService.addIncome(newIncome).subscribe(addedIncome => {
                if (!addedIncome) {
                    this.snackBar.open('Не удалось сохранить данные по доходу!', 'OK', { duration: 3000 });
                    return;
                }

                newIncome.date = addedIncome.date.substring(0, 10);
                newIncome.id = addedIncome.id;
                newIncome.amountToDisplay = addedIncome.amountToDisplay;
                newIncome.shortComment = addedIncome.shortComment;

                this.incomes.push(newIncome);
                this.dataSource = new MatTableDataSource(this.incomes);
                this.selectedIncome = null;
                this.setControlsWithDefaultsValues();

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
                comment: this.commentControl.value,
                amountToDisplay: this.selectedIncome.amountToDisplay,
                shortComment: this.selectedIncome.shortComment
            }

            this.incomesService.editIncome(editedIncome).subscribe(editedIncomeData => {
                if (!editedIncomeData) {
                    this.snackBar.open('Не удалось сохранить данные по доходу!', 'OK', { duration: 3000 });
                    return;
                }

                editedIncome.amountToDisplay = editedIncomeData.amountToDisplay;
                this.incomes[this.incomes.findIndex(i => i.id === editedIncomeData.id)] = editedIncome;
                this.dataSource = new MatTableDataSource(this.incomes);
                this.selectedIncome = null;
                this.incomeFormGroup.reset();
                this.setControlsWithDefaultsValues();
            });
        }

    }

    editIncome(income: Income) {
        if (!income || !income.id) {
            return;
        }

        this.selectedIncome = income;
        this.incomeAmountControl.setValue(this.selectedIncome.amount);
        this.commentControl.setValue(this.selectedIncome.comment);
        this.incomeDateControl.setValue(moment(this.selectedIncome.date));

        const payType = new PayType();
        payType.id = this.selectedIncome.payTypeId;
        payType.name = this.selectedIncome.payType;

        this.incomeTypeControl.setValue(payType);
    }

    deleteIncome(income: Income) {
        if (!income || !income.id) {
            return;
        }

        const confirmationDialog = this.dialogRef.open(ConfirmationComponent, { data: { caption: 'Внимание!', message: 'Вы уверены, что хотите удалить данные по доходу?' } });
        confirmationDialog.afterClosed()
            .subscribe(res => {
                if (!res) {
                    return;
                }

                this.incomesService.deleteIncome(income.id).subscribe(() => {
                    this.incomes = this.incomes.filter(i => i.id !== income.id);
                    this.dataSource = new MatTableDataSource(this.incomes);
                }, error => {
                    this.snackBar.open('Не удалось удалить данные дохода!', 'OK', { duration: 3000 });
                });
            });

    }

    displayIncomeType(payType: PayType) {
        return payType && payType.name ? payType.name : '';
    }

    getTotalIncomes() {
        let total = 0;
        this.incomes.map(v => total += v.amount)

        return total;
    }

    private filterIncome(value: string) {
        const filteredValue = value.toLowerCase();
        return this.payTypes.filter(pt => pt.name.toLowerCase().includes(filteredValue));
    }

    private getPreviousMonthbeginDate() {
        // let currentDate = moment();
        // let previousMonth = currentDate.month();
        // let year = currentDate.year();
        return moment().add(-1, 'M');
    }

    private setControlsWithDefaultsValues() {
        this.incomeAmountControl = new FormControl(0, [Validators.required, Validators.min(1)]);

        this.commentControl = new FormControl('', [Validators.maxLength(2000)]);
        this.incomeTypeControl = new FormControl('', [Validators.required]);

        this.incomeDateControl = new FormControl(moment(), [Validators.required]);

        this.incomeFormGroup = new FormGroup({
            incomeAmount: this.incomeAmountControl,
            incomeType: this.incomeTypeControl,
            incomeDate: this.incomeDateControl,
            comment: this.commentControl
        });
    }
}