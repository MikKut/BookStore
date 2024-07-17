import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private apiUrl = environment.backendReportUrl;

  constructor(private http: HttpClient) {}

  getBooksPublicationData(): Observable<{ [year: number]: number }> {
    console.log('Fetching publication data from:', `${this.apiUrl}/GetBooksPublicationData`);
    return this.http.get<{ [year: number]: number }>(`${this.apiUrl}/GetBooksPublicationData`);
  }
}
