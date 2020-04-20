import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Cost } from './cost.model';

@Injectable()
export class CostsService {

    getCosts(): Observable<Cost> {

    }

    addCost(cost: Cost): Observable<Cost> {

    }

    updateCost(cost: Cost): Observable<Cost> {

    }

    deleteCost(cost: Cost): void {

    }
}
