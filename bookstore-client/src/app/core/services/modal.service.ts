import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Book } from '../models/book.model';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private isOpenSubject = new BehaviorSubject<boolean>(false);
  private bookSubject = new BehaviorSubject<Book | null>(null);

  isOpen$ = this.isOpenSubject.asObservable();
  book$ = this.bookSubject.asObservable();

  open(book: Book | null = null) {
    this.bookSubject.next(book);
    this.isOpenSubject.next(true);
  }

  close() {
    this.isOpenSubject.next(false);
  }
}
