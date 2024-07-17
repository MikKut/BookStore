import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'app-sort-dropdown',
  templateUrl: './sort-dropdown.component.html',
  styleUrls: ['./sort-dropdown.component.css']
})
export class SortDropdownComponent {
  @Output() sort = new EventEmitter<string>();

  onSortChange(event: Event): void { 
    const selectElement = event.target as HTMLSelectElement;
    this.sort.emit(selectElement.value);
  }
}
