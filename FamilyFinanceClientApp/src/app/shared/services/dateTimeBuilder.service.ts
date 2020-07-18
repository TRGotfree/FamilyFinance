import { Injectable } from '@angular/core';
@Injectable()
export class DateTimeBuilder {

  public getFormattedDate(date: Date, divider= '') {
    const year = date.getFullYear().toString();
    const monthNumber = date.getMonth() + 1;
    const month = monthNumber.toString().length === 1 ?
      '0' + monthNumber.toString() : monthNumber.toString();
    const day = date.getDate().toString().length === 1 ? '0' + date.getDate().toString() : date.getDate().toString();

    return `${year}${divider}${month}${divider}${day}`;
  }
}
