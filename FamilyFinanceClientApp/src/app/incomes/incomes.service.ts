import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CustomLogger } from '../shared/services/logger.service';
import { DateTimeBuilder } from '../shared/services/dateTimeBuilder.service';
import { Income } from './income.model';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class IncomesService {
    constructor(private httpClient: HttpClient, private logger: CustomLogger, private dateTimeBuilder: DateTimeBuilder) { }

    getIncomes(beginDate: Date, endDate: Date): Observable<Income[]> {
        let httpParams = new HttpParams();
        httpParams = httpParams.append('beginDate', this.dateTimeBuilder.getFormattedDate(beginDate, '-'));
        httpParams = httpParams.append('endDate', this.dateTimeBuilder.getFormattedDate(endDate, '-'));

        return this.httpClient.get<Income[]>('/api/incomes', { params: httpParams })
            .pipe(catchError(this.errorHandler<Income[]>('getIncomes')));
    }

    addIncome(income: Income): Observable<Income> {
        return this.httpClient.post<Income>('/api/incomes', income)
            .pipe(catchError(this.errorHandler<Income>('addIncome')));
    }

    editIncome(income: Income): Observable<Income> {
        return this.httpClient.post<Income>('/api/incomes', income)
            .pipe(catchError(this.errorHandler<Income>('editIncome')));
    }

    deleteIncome(incomeId: number): Observable<any> {
        let httpParams = new HttpParams();
        httpParams = httpParams.append('incomeId', incomeId + '');

        return this.httpClient.delete('/api/incomes', { params: httpParams });
    }

    private errorHandler<T>(operation = 'unknown operation', result?: T) {
        return (error: any): Observable<T> => {
            this.logger.logError('Error during: ' + operation + ' Details: ' + error);
            return of(result as T);
        };
    }
}