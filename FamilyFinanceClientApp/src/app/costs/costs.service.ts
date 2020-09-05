import { DateTimeBuilder } from './../shared/services/dateTimeBuilder.service';
import { CustomLogger } from './../shared/services/logger.service';
import { catchError } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Cost } from './cost.model';
import { TableColumnAttribute } from '../shared/models/tableColumnAttribute';
import { throwCustomError } from '../shared/services/server.error.catcher';
import { ServerResponseError } from '../shared/models/server.response.error.model';
@Injectable()
export class CostsService {

  constructor(private httpClient: HttpClient, private logger: CustomLogger, private dateTimeBuilder: DateTimeBuilder) {
  }

  getCosts(date: Date): Observable<Cost[] | ServerResponseError> {
    return this.httpClient.get<Cost[]>(`/api/costs/${this.dateTimeBuilder.getFormattedDate(date, '-')}`)
      .pipe(catchError(throwCustomError));
  }

  getCostsMeta(): Observable<TableColumnAttribute[] | ServerResponseError> {
    return this.httpClient.get<TableColumnAttribute[]>('/api/costs/meta')
      .pipe(catchError(throwCustomError));
  }

  addCost(cost: Cost): Observable<Cost | ServerResponseError> {
    return this.httpClient.post<Cost>('/api/costs', cost)
      .pipe(catchError(throwCustomError));
  }

  updateCost(cost: Cost): Observable<Cost | ServerResponseError> {
    return this.httpClient.put<Cost>('/api/costs', cost)
      .pipe(catchError(throwCustomError));
  }

  deleteCost(cost: Cost): Observable<any> {
    return this.httpClient.delete('/api/costs/' + cost.id)
      .pipe(catchError(throwCustomError));
  }

}
