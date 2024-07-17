import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book } from '../models/book.model';
import { BooksPagedFilterRequest } from '../models/book-filter.model';
import { environment } from '../../../environments/environment';
import * as XLSX from 'xlsx';
import jsPDF from 'jspdf';
import 'jspdf-autotable';
import { PagedResponse } from '../responses/paged-response';
@Injectable({
  providedIn: 'root'
})
export class BookService {
  private apiUrl = environment.backendBookUrl;

  constructor(private http: HttpClient) {}

  getBooks(filterRequest?: BooksPagedFilterRequest): Observable<PagedResponse<Book>> {
    
    let params = new HttpParams();
    if (filterRequest) {
      params = params.set('pageNumber', filterRequest.pageNumber)
      .set('pageSize', filterRequest.pageSize);
      if (filterRequest.name) {
        params = params.set('name', filterRequest.name);
      }
      if (filterRequest.startDate) {
        params = params.set('startDate', filterRequest.startDate);
      }
      if (filterRequest.endDate) {
        params = params.set('endDate', filterRequest.endDate);
      }
    }
    return this.http.get<PagedResponse<Book>>(`${this.apiUrl}/GetBooks`, { params });
  }

  getBookById(id: number): Observable<Book> {
    return this.http.get<Book>(`${this.apiUrl}/GetBook/${id}`);
  }

  addBook(book: Book): Observable<Book> {
    return this.http.post<Book>(`${this.apiUrl}/AddBook`, book);
  }

  updateBook(book: Book): Observable<Book> {
    console.log("update: ", book);
    return this.http.put<Book>(`${this.apiUrl}/UpdateBook/${book.id}`, book);
  }

  deleteBook(id: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/DeleteBook/${id}`);
  }

  exportBooksToExcel(filteredBooks: Book[]): void {
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(filteredBooks);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Books');
    XLSX.writeFile(wb, 'books.xlsx');
  }

  exportBooksToPDF(filteredBooks: Book[]): void {
    const doc = new jsPDF();
    const col = ['Title', 'Publication Date', 'Number of Pages', 'Description'];
    const rows = filteredBooks.map(book => [
      book.title, 
      new Date(book.publicationDate).toLocaleDateString(), 
      book.numberOfPages, 
      book.description
    ]);
    (doc as any).autoTable({ head: [col], body: rows });
    doc.save('books.pdf');
  }

}
