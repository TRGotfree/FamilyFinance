import { CustomLogger } from './../../common/logger.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Store } from './store.model';
import { catchError } from 'rxjs/operators';

@Injectable()
export class StoreService {
  constructor(private httpClient: HttpClient, private logger: CustomLogger) {

  }

  public getStores(): Observable<Store[]> {
    return this.httpClient.get<Store[]>('/api/stores')
      .pipe(catchError(this.handleError<Store[]>('getStores', [])));
  }

  public addStore(newStore: Store): Observable<Store> {
    return this.httpClient.post<Store>('/api/stores', newStore)
      .pipe(catchError(this.handleError<Store>('addStore', null)));
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      this.logger.logError('Error during: ' + operation + ' Details: ' + error);
      return of(result as T);
    };
  }

}
