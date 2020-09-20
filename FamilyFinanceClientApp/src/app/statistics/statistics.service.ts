import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CustomLogger } from '../shared/services/logger.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Statistics } from './statistics.model';

@Injectable({
    providedIn: 'root'
})
export class StatisticsService {
  constructor(private httpClient: HttpClient, private logger: CustomLogger) { }

  private url = '/api/statistics';

  public getStatistics(month: number, year: number): Observable<Statistics> {
    // let urlParams = new HttpParams();
    // urlParams = urlParams.append('month', month.toString());
    // urlParams = urlParams.append('year', year.toString());

    return this.httpClient.get<Statistics>(this.url + '/' + month + '/' + year)
      .pipe(catchError(this.errorHandler<Statistics>('getStatistics', null)));
  }


  private errorHandler<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      this.logger.logError('Error during: ' + operation + ' Details: ' + error);
      return of(result as T);
    };
  }
}
