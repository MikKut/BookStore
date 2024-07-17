import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BookListComponent } from './features/book-management/book-list/book-list.component';
import { BookDetailComponent } from './features/book-management/book-detail/book-detail.component';
import { BookFormComponent } from './features/book-management/book-form/book-form.component';
import { PublicationChartComponent } from './features/reports/publication-chart/publication-chart.component';

export const routes: Routes = [
  { path: '', redirectTo: '/books', pathMatch: 'full' },
  { path: 'books', component: BookListComponent },
  { path: 'books/new', component: BookFormComponent },
  { path: 'books/:id', component: BookDetailComponent },
  { path: 'books/:id/edit', component: BookFormComponent },
  { path: 'publication-chart', component: PublicationChartComponent },
  { path: '**', redirectTo: '/books' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
