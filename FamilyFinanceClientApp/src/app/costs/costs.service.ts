import { DateTimeBuilder } from './../shared/services/dateTimeBuilder.service';
import { CustomLogger } from './../shared/services/logger.service';
import { catchError } from 'rxjs/operators';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Cost } from './cost.model';
import { TableColumnAttribute } from '../shared/models/tableColumnAttribute';

@Injectable()
export class CostsService {

  constructor(private httpClient: HttpClient, private logger: CustomLogger, private dateTimeBuilder: DateTimeBuilder) {
  }

  getCosts(date: Date): Observable<Cost[]> {
    return this.httpClient.get<Cost[]>(`/api/costs/${this.dateTimeBuilder.getCurrentDate(date, '-')}`)
      .pipe(catchError(this.errorHandler<Cost[]>('getCosts', [])));
  }

  getCostsMeta(): Observable<TableColumnAttribute[]> {
    return this.httpClient.get<TableColumnAttribute[]>('/api/costs/meta')
      .pipe(catchError(this.errorHandler<TableColumnAttribute[]>('getCostsMeta', [])));
  }

  addCost(cost: Cost): Observable<Cost> {
    return this.httpClient.post<Cost>('/api/costs', cost)
      .pipe(catchError(this.errorHandler<Cost>('addCost', null)));
  }

  updateCost(cost: Cost): Observable<Cost> {
    return this.httpClient.put<Cost>('/api/costs', cost)
      .pipe(catchError(this.errorHandler<Cost>('updateCost', null)));
  }

  deleteCost(cost: Cost): Observable<void> {
    return this.httpClient.delete('/api/costs/' + cost.id)
      .pipe(catchError(this.errorHandler('deleteCost', undefined)));
  }

  private errorHandler<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      this.logger.logError('Error during: ' + operation + ' Details: ' + error);
      return of(result as T);
    };
  }
}
