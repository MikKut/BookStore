import { Component, EventEmitter, Output, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Book } from '../../../core/models/book.model';
import { ModalService } from '../../../core/services/modal.service';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css']
})
export class ModalComponent implements OnChanges {
  @Output() save = new EventEmitter<Book>();
  @Output() cancel = new EventEmitter<void>();

  bookForm: FormGroup;
  book: Book | null = null;
  isOpen: boolean = false;

  constructor(private fb: FormBuilder, private modalService: ModalService) {
    this.bookForm = this.fb.group({
      title: ['', Validators.required],
      publicationDate: ['', Validators.required],
      description: ['', Validators.required],
      numberOfPages: ['', [Validators.required, Validators.min(1)]]
    });

    this.modalService.book$.subscribe(book => {
      this.book = book;
      if (this.book) {
        this.bookForm.patchValue(this.book);
      }
    });

    this.modalService.isOpen$.subscribe(isOpen => {
      this.isOpen = isOpen;
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['book'] && this.book) {
      this.bookForm.patchValue(this.book);
    }
  }

  onSubmit() {
    if (this.bookForm.valid) {
      this.save.emit(this.bookForm.value);
    }
  }

  onCancel() {
    this.cancel.emit();
    this.modalService.close();
  }

  onBackdropClick(event: MouseEvent) {
    if ((event.target as HTMLElement).classList.contains('modal-backdrop')) {
      this.onCancel();
    }
  }
}
