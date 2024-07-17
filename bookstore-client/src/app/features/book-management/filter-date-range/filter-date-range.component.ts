import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  selector: 'app-filter-date-range',
  templateUrl: './filter-date-range.component.html',
  styleUrls: ['./filter-date-range.component.css']
})
export class FilterDateRangeComponent {
  startDate: string = '';
  endDate: string = '';

  @Output() filter = new EventEmitter<{ startDate: string, endDate: string }>();

  onFilterChange(): void {
    this.filter.emit({ startDate: this.startDate, endDate: this.endDate });
  }

  setDateRange(period: string): void {
    const currentDate = new Date();
    if (period === 'thisMonth') {
      this.startDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1).toISOString().split('T')[0];
      this.endDate = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0).toISOString().split('T')[0];
    } else if (period === 'thisYear') {
      this.startDate = new Date(currentDate.getFullYear(), 0, 1).toISOString().split('T')[0];
      this.endDate = new Date(currentDate.getFullYear(), 11, 31).toISOString().split('T')[0];
    }
    this.onFilterChange();
  }
}
