import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';  

@Injectable({
  providedIn: 'root'
})
export class CurrencyService {
  private apiUrl = 'https://localhost:7250/api/currency';

  constructor(private http: HttpClient) { }

  getConvertedAmount(from: string, to: string, amount: number, date: string): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/convert?fromCurrency=${from}&toCurrency=${to}&amount=${amount}&date=${date}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  getRates(date: string): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/rates?date=${date}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred!';
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred.
      errorMessage = `An error occurred: ${error.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      if (error.status === 404) {
        errorMessage = 'Currency rates file not found for the given date';
      } else {
        errorMessage = `Server returned code: ${error.status}, Something is not selected`;
      }
    }
    return throwError(errorMessage);
  }
}
