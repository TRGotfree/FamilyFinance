import { CustomLogger } from './../common/logger.service';
import { catchError } from 'rxjs/operators';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Cost } from './cost.model';
import { CostsTableColumn } from './costsTableColumns.model';

@Injectable()
export class CostsService {

  constructor(private httpClient: HttpClient, private logger: CustomLogger) {
  }

  getCosts(date: Date): Observable<Cost[]> {
    return this.httpClient.get<Cost[]>(`/api/costs/${date.toISOString().substring(0, 10)}`)
      .pipe(catchError(this.errorHandler<Cost[]>('getCosts', [])));
  }

  getCostsMeta(): Observable<CostsTableColumn[]> {
    return this.httpClient.get<CostsTableColumn[]>('/api/costs/meta')
      .pipe(catchError(this.errorHandler<CostsTableColumn[]>('getCostsMeta', [])));
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
      this.logger.logError('Error during: ' + operation + ' Details: ' + error);
      return of(result as T);
    };
  }
}
