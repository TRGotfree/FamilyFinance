import { Injectable } from '@angular/core';
import { HttpClient, HttpBackend, HttpParams } from '@angular/common/http';
import { CustomLogger } from 'src/app/shared/services/logger.service';
import { Observable, of } from 'rxjs';
import { Category } from './category.model';
import { catchError } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class CategoryService {
    constructor(private httpClient: HttpClient, private logger: CustomLogger) { }

    getCategories(): Observable<Category[]> {
        return this.httpClient.get<Category[]>('/api/categories')
            .pipe(catchError(this.errorHandler<Category[]>('getCategories')));
    }

    addCategory(category: Category): Observable<Category> {
        return this.httpClient.post<Category>('/api/categories', category)
        .pipe(catchError(this.errorHandler<Category>('addCategory')))
    }

    editCategory(category: Category): Observable<Category> {
        return this.httpClient.put<Category>('/api/categories', category)
        .pipe(catchError(this.errorHandler<Category>('editCategory')));
    }

    deleteCategory(categoryId: number): Observable<any> {
        const httpParams = new HttpParams().set('id', categoryId + '');
        return this.httpClient.delete('/api/categories', { params: httpParams });
    }

    private errorHandler<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            this.logger.logError('Error during: ' + operation + ' Details: ' + error);
            return of(result as T);
        };
    }
}