import { catchError } from 'rxjs/operators';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { Cost } from './cost.model';

@Injectable()
export class CostsService {

  constructor(private httpClient: HttpClient) {
  }

  getCosts(date: Date): Observable<Cost[]> {
    return this.httpClient.get<Cost[]>(`/api/costs/${date}`)
      .pipe(catchError(this.errorHandler<Cost[]>('getCosts', [])));
  }

  addCost(cost: Cost): Observable<Cost> {
    return this.httpClient.post<Cost>('/api/costs', cost)
      .pipe(catchError(this.errorHandler<Cost>('addCost', new Cost())));
  }

  updateCost(cost: Cost): Observable<Cost> {
    return this.httpClient.put<Cost>('/api/costs', cost)
      .pipe(catchError(this.errorHandler<Cost>('addCost', new Cost())));
  }

  deleteCost(cost: Cost): Observable<void> {
    return this.httpClient.delete('/api/costs/' + cost.id)
      .pipe(catchError(this.errorHandler('addCost', undefined)));
  }

  private errorHandler<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.debug(error);
      return of(result as T);
    };
  }
}
