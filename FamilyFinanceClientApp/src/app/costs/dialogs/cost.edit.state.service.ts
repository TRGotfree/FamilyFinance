import { Injectable } from '@angular/core';
import { threadId } from 'worker_threads';

@Injectable({providedIn: 'root'})
export class CostEditStateService {
    constructor() { }
    
    private _lastStore: string;
    private _lastPayType: string;

    get lastStore(): string {
        return this._lastStore;
    }

    set lastStore(value: string) {
        this._lastStore = value;
    }

    get lastPayType(): string {
        return this._lastPayType;
    }

    set lastPayType(value: string) {
        this._lastPayType = value;
    }
}