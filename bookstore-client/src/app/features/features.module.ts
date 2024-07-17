import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BookListComponent } from './book-management/book-list/book-list.component';
import { BookDetailComponent } from './book-management/book-detail/book-detail.component';
import { BookFormComponent } from './book-management/book-form/book-form.component';
import { PublicationChartComponent } from './reports/publication-chart/publication-chart.component';
import { SearchBarComponent } from './book-management/search-bar/search-bar.component';
import { SortDropdownComponent } from './book-management/sort-dropdown/sort-dropdown.component';
import { FilterDateRangeComponent } from './book-management/filter-date-range/filter-date-range.component';
import { ModalComponent } from '../shared/components/modal/modal.component';
import { HttpClient, HttpClientModule, provideHttpClient } from '@angular/common/http';
import { BookService } from '../core/services/book.service';
import { ReportService } from '../core/services/report.service';
import { SharedModule } from '../shared/shared.module';
@NgModule({
  declarations: [
    PublicationChartComponent,
    BookListComponent,
    BookFormComponent
  ],
  imports: [
    HttpClientModule,
    SearchBarComponent, 
    SortDropdownComponent, 
    FilterDateRangeComponent,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule, 
    BookDetailComponent
  ],
  exports: [
    PublicationChartComponent,
    BookListComponent,
    BookFormComponent,
  ],
  providers: [provideHttpClient()],
})
export class FeaturesModule { }
