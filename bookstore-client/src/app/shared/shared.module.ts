import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { DateRangePipe } from './pipes/date-range.pipe';
import { HighlightDirective } from './directives/highlight.directive';
import { ModalComponent } from './components/modal/modal.component';

@NgModule({
  declarations: [
    DateRangePipe,
    HighlightDirective,
    ModalComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [
    ReactiveFormsModule,
    DateRangePipe,
    HighlightDirective,
    ModalComponent
  ]
})
export class SharedModule { }
