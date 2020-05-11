import { TableColumnAttribute } from './../shared/models/tableColumnAttribute';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CustomLogger } from '../shared/services/logger.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Plan } from './plan.model';

@Injectable()
export class PlanService {
  constructor(private httpClient: HttpClient, private logger: CustomLogger) { }

  public getPlans(month: number, year: number): Observable<Plan[]> {
    const urlParams = new HttpParams();
    urlParams.append('month', month.toString());
    urlParams.append('year', year.toString());

    return this.httpClient.get<Plan[]>('/api/plans', { params: urlParams })
    .pipe(catchError(this.errorHandler<Plan[]>('getPlans', null)));
  }

  public getPlansMeta() {
    return this.httpClient.get<TableColumnAttribute[]>('/api/plans/meta')
    .pipe(catchError(this.errorHandler<TableColumnAttribute[]>('getPlansMeta', null)));
  }

  private errorHandler<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      this.logger.logError('Error during: ' + operation + ' Details: ' + error);
      return of(result as T);
    };
  }
}
