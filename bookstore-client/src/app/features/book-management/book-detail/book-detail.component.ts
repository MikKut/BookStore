import { Component, Input, OnChanges, SimpleChanges, EventEmitter, Output } from '@angular/core';
import { Book } from '../../../core/models/book.model';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  imports: [FormsModule, CommonModule, ReactiveFormsModule],
  selector: 'app-book-detail',
  templateUrl: './book-detail.component.html',
  styleUrls: ['./book-detail.component.css']
})
export class BookDetailComponent implements OnChanges {
  @Input() book: Book | null = null;
  @Output() saveBook = new EventEmitter<Book>();
  @Output() cancelEdit = new EventEmitter<void>();
  editedBook: Book = this.initializeBook();

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['book']) {
      this.editedBook = this.book ? { ...this.book } : this.initializeBook();
    }
  }

  onCancel(): void {
    this.cancelEdit.emit();
    if (this.book) {
      this.editedBook = { ...this.book };
    }
  }

  onSave(): void {
    if (this.editedBook) {
      this.saveBook.emit(this.editedBook);
    }
  }

  private initializeBook(): Book {
    return {
      id: 0,
      title: '',
      publicationDate: new Date(),
      description: '',
      numberOfPages: 0
    };
  }
}
