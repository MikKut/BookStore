import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { BookService } from '../../../core/services/book.service';
import { Book } from '../../../core/models/book.model';

@Component({
  selector: 'app-book-form',
  templateUrl: './book-form.component.html',
  styleUrls: ['./book-form.component.css']
})
export class BookFormComponent implements OnInit {
  bookForm: FormGroup;
  bookId: number | null = null;
  isEditMode: boolean = false;

  constructor(
    private fb: FormBuilder,
    private bookService: BookService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.bookForm = this.fb.group({
      title: ['', Validators.required],
      publicationDate: ['', Validators.required],
      description: ['', [Validators.required, Validators.maxLength(500)]],
      numberOfPages: ['', [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.bookId = +id;
        this.isEditMode = true;
        this.loadBook();
      }
    });
  }

  loadBook(): void {
    if (this.bookId !== null) {
      this.bookService.getBookById(this.bookId).subscribe(book => {
        if (book.publicationDate) {
          // Convert the Date to the string format required by the form
          const publicationDateString = new Date(book.publicationDate).toISOString().split('T')[0];
          this.bookForm.patchValue({
            ...book,
            publicationDate: publicationDateString
          });
        } else {
          this.bookForm.patchValue(book);
        }
      });
    }
  }

  onSubmit(): void {
    if (this.bookForm.invalid) {
      return;
    }

    const formValue = this.bookForm.value;
    const book: Book = {
      ...formValue,
      publicationDate: new Date(formValue.publicationDate)
    };
    
    console.log("on submit", book);
    
    if (this.isEditMode && this.bookId !== null) {
      book.id = this.bookId;
      this.bookService.updateBook(book).subscribe(() => {
        this.router.navigate(['/books']);
      });
    } else {
      book.id = 0;
      this.bookService.addBook(book).subscribe(() => {
        this.router.navigate(['/books']);
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/books']);
  }

  get title() {
    return this.bookForm.get('title');
  }

  get publicationDate() {
    return this.bookForm.get('publicationDate');
  }

  get description() {
    return this.bookForm.get('description');
  }

  get numberOfPages() {
    return this.bookForm.get('numberOfPages');
  }
}
