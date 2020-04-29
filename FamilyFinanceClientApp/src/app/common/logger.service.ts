import { environment } from './../../environments/environment.prod';
import { Injectable } from '@angular/core';

@Injectable()
export class CustomLogger {
  logError(message: string): void {
    if (!environment.production) {
      console.error(message);
    }
  }

  logInfo(message: string): void {
    if (!environment.production) {
      console.log(message);
    }
  }
}
