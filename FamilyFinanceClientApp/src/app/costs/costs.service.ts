import { catchError } from 'rxjs/operators';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable, of } from 'rxjs';
import { Cost } from './cost.model';
import { CostsTableColumns } from './costsTableColumns.model';

@Injectable()
export class CostsService {

  constructor(private httpClient: HttpClient) {
  }

  getCosts(date: Date): Observable<Cost[]> {
    return this.httpClient.get<Cost[]>(`/api/costs/${date}`)
      .pipe(catchError(this.errorHandler<Cost[]>('getCosts', [])));
  }

  getCostsMeta(): Observable<CostsTableColumns[]> {
    return this.httpClient.get<CostsTableColumns[]>('/api/costs/meta')
      .pipe(catchError(this.errorHandler<CostsTableColumns[]>('getCostsMeta', [])));
  }

  addCost(cost: Cost): Observable<Cost> {
    return this.httpClient.post<Cost>('/api/costs', cost)
      .pipe(catchError(this.errorHandler<Cost>('addCost', new Cost())));
  }

  updateCost(cost: Cost): Observable<Cost> {
    return this.httpClient.put<Cost>('/api/costs', cost)
      .pipe(catchError(this.errorHandler<Cost>('updateCost', new Cost())));
  }

  deleteCost(cost: Cost): Observable<void> {
    return this.httpClient.delete('/api/costs/' + cost.id)
      .pipe(catchError(this.errorHandler('deleteCost', undefined)));
  }

  private errorHandler<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      if (!environment.production) {
        console.error(error);
      }
      return of(result as T);
    };
  }
}
