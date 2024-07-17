export interface BooksPagedFilterRequest {
    startDate?: string;
    endDate?: string;
    name?: string;
    pageNumber: number;
    pageSize: number;
  }
  