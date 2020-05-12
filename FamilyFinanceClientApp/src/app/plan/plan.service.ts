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

  private url = '/api/plans';

  public getPlans(month: number, year: number): Observable<Plan[]> {
    const urlParams = new HttpParams();
    urlParams.append('month', month.toString());
    urlParams.append('year', year.toString());

    return this.httpClient.get<Plan[]>(this.url, { params: urlParams })
      .pipe(catchError(this.errorHandler<Plan[]>('getPlans', null)));
  }

  public getPlansMeta() {
    return this.httpClient.get<TableColumnAttribute[]>(this.url + '/meta')
      .pipe(catchError(this.errorHandler<TableColumnAttribute[]>('getPlansMeta', null)));
  }

  public addPlan(plan: Plan): Observable<Plan> {
    if (!plan) {
      throw new Error('Input parameter plan couldn\'t be null or undefined!');
    }

    return this.httpClient.post<Plan>(this.url, plan)
      .pipe(catchError(this.errorHandler<Plan>('addPlan', null)));
  }

  public updatePlan(plan: Plan): Observable<Plan> {
    if (!plan) {
      throw new Error('Input parameter plan couldn\'t be null or undefined!');
    }
    return this.httpClient.put<Plan>(this.url, plan)
      .pipe(catchError(this.errorHandler<Plan>('updatePlan', null)));
  }

  public deletePlan(id: number): Observable<void> {
    return this.httpClient.delete<void>(`${this.url}/${id}`)
      .pipe(catchError(this.errorHandler<void>('deletePlan', null)));
  }

  private errorHandler<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      this.logger.logError('Error during: ' + operation + ' Details: ' + error);
      return of(result as T);
    };
  }
}
