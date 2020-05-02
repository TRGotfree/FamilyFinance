import { CustomLogger } from './../../shared/services/logger.service';
import { PayType } from './paytype.model';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class PayTypeService {
  constructor(private httpClient: HttpClient, private logger: CustomLogger) { }

  public getPayTypes(): Observable<PayType[]> {
    return this.httpClient.get<PayType[]>('/api/paytypes')
      .pipe(catchError(this.handleError<PayType[]>('getPayTypes', [])));
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      this.logger.logError('Error during: ' + operation + ' Details: ' + error);
      return of(result as T);
    };
  }
}
