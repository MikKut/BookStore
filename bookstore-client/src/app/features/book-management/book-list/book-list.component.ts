import { Component, OnInit } from '@angular/core';
import { BookService } from '../../../core/services/book.service';
import { Book } from '../../../core/models/book.model';
import { HttpErrorResponse } from '@angular/common/http';
import { ModalService } from '../../../core/services/modal.service';
import { ReportService } from '../../../core/services/report.service';
import { Router } from '@angular/router';
import { BooksPagedFilterRequest } from '../../../core/models/book-filter.model';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {
  books: Book[] = [];
  filteredBooks: Book[] = [];
  selectedBook: Book | null = null;
  searchQuery: string = '';
  sortCriteria: string = '';
  filterDateRange: { startDate: string, endDate: string } = { startDate: '', endDate: '' };
  isModalOpen: boolean = false;
  isAddModalOpen = false;
  isEditModalOpen = false;
  modalBook: Book | null = null;
  pageNumber: number = 1;
  pageSize: number = 10;
  totalCount: number = 0;
  totalPages: number = 0;
  searchText: string = '';
  startDate: string = '';
  endDate: string = '';

  constructor(
    private bookService: BookService, 
    public modalService: ModalService,
    private router: Router ) { }

  ngOnInit(): void {
    this.loadBooks();
  }

  onPageChange(pageNumber: number): void {
    if (pageNumber > 0 && pageNumber <= this.totalPages) {
      this.pageNumber = pageNumber;
      this.loadBooks();
    }
  }

  loadBooks(): void {
    const filterRequest: BooksPagedFilterRequest = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      startDate: this.startDate,
      endDate: this.endDate,
      name: this.searchText,  
    };

    console.log(filterRequest);
    this.bookService.getBooks(filterRequest).subscribe(response => {
      this.books = response?.items ?? [];
      this.totalCount = response?.totalCount ?? 0;
      this.totalPages = Math.ceil(this.totalCount / this.pageSize);
      this.applyFilters();
    });

    console.log("books:", this.books);
  }

  applyFilters(): void {
    this.filteredBooks = this.books
      .filter(book => book.title.toLowerCase().includes(this.searchQuery.toLowerCase()))
      .sort((a, b) => this.sortBooks(a, b))
      .filter(book => this.filterByDateRange(book));
  }

  sortBooks(a: Book, b: Book): number {
    if (!this.sortCriteria) return 0;
    if (this.sortCriteria === 'title') return a.title.localeCompare(b.title);
    if (this.sortCriteria === 'publicationDate') return new Date(a.publicationDate).getTime() - new Date(b.publicationDate).getTime();
    if (this.sortCriteria === 'numberOfPages') return a.numberOfPages - b.numberOfPages;
    return 0;
  }

  filterByDateRange(book: Book): boolean {
    const publicationDate = new Date(book.publicationDate).toISOString().split('T')[0];
    return (!this.filterDateRange.startDate || publicationDate >= this.filterDateRange.startDate) &&
           (!this.filterDateRange.endDate || publicationDate <= this.filterDateRange.endDate);
  }

  onSort(criteria: string): void {
    this.sortCriteria = criteria;
    this.applyFilters();
  }

  onFilterDateRange(dateRange: { startDate: string, endDate: string }): void {
    this.startDate = dateRange.startDate;
    this.endDate = dateRange.endDate;
    this.pageNumber = 1;
    this.loadBooks();
  }

  onSelectBook(book: Book): void {
    this.selectedBook = book;
    this.isModalOpen = false;
  }

  openAddModal(): void {
    this.router.navigate(['/books/new']);
  }

  openEditModal(book: Book): void {
    this.router.navigate([`/books/${book.id}/edit`]);
  }

  onSave(book: Book): void {
    console.log("onSave: ", book);
    if (this.isEditModalOpen) {
      this.bookService.updateBook(book).subscribe(() => this.loadBooks());
    } else {
      this.bookService.addBook(book).subscribe(() => this.loadBooks());
    }
    this.onCancel();
  }

  onSearch(query: string): void {
    this.searchText = query;
    this.pageNumber = 1;
    this.loadBooks();
  }

  onCancel(): void {
    this.isAddModalOpen = false;
    this.isEditModalOpen = false;
  }

  onDeleteBook(book: Book): void {
    if (confirm(`Are you sure you want to delete the book "${book.title}"?`)) {
      this.bookService.deleteBook(book.id).subscribe(
        () => this.loadBooks(),
        (error: HttpErrorResponse) => console.error('Error deleting book:', error)
      );
    }
  }

  viewPublicationChart(): void {
    this.router.navigate(['/publication-chart']); 
  }

  exportToExcel(): void {
    this.bookService.exportBooksToExcel(this.filteredBooks);
  }

  exportToPDF(): void {
    this.bookService.exportBooksToPDF(this.filteredBooks);
  }
}
