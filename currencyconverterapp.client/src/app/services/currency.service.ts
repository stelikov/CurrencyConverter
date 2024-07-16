import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CurrencyService {
  private apiUrl = 'https://localhost:7250/api/currency';

  constructor(private http: HttpClient) { }


  getConvertedAmount(from: string, to: string, amount: number, date: string): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/convert?fromCurrency=${from}&toCurrency=${to}&amount=${amount}&date=${date}`);
  }
}
