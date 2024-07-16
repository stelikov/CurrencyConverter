import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CurrencyService } from '../services/currency.service';

@Component({
  selector: 'app-currency-converter',
  templateUrl: './currency-converter.component.html',
  styleUrls: ['./currency-converter.component.css']
})
export class CurrencyConverterComponent {
  converterForm: FormGroup;
  convertedAmount: number;
  currencies: string[] = ['AUD', 'BGN', 'BRL', 'CAD', 'CHF', 'CNY', 'CZK', 'DKK', 'GBP', 'HKD', 'HUF', 'IDR', 'ILS', 'INR', 'ISK', 'JPY', 'KRW', 'MXN', 'MYR', 'NOK', 'NZD', 'PHP', 'PLN', 'RON', 'SEK', 'SGD', 'THB', 'TRY', 'USD', 'ZAR'];

  constructor(private fb: FormBuilder, private currencyService: CurrencyService) {
    this.convertedAmount = 0;
    this.converterForm = this.fb.group({
      fromCurrency: [''],
      toCurrency: [''],
      amount: [''],
      date: ['']
    });
  }

  onSubmit(): void {
    const { fromCurrency, toCurrency, amount, date } = this.converterForm.value;
    this.currencyService.getConvertedAmount(fromCurrency, toCurrency, amount, date)
      .subscribe(result => this.convertedAmount = result);
  }

  onReset(): void {
    this.converterForm.reset();
    this.convertedAmount = 0;
  }
}
