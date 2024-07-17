import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dateRange'
})
export class DateRangePipe implements PipeTransform {

  transform(items: any[], startDate: Date, endDate: Date): any[] {
    if (!items || !startDate || !endDate) {
      return items;
    }

    startDate = new Date(startDate);
    endDate = new Date(endDate);

    return items.filter(item => {
      const itemDate = new Date(item.publicationDate);
      return itemDate >= startDate && itemDate <= endDate;
    });
  }
}
