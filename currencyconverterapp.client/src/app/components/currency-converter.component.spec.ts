import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CurrencyConverterComponent } from './currency-converter.component';

describe('CurrencyConverterComponent', () => {
  let component: CurrencyConverterComponent;
  let fixture: ComponentFixture<CurrencyConverterComponent>;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CurrencyConverterComponent],
      imports: [HttpClientTestingModule]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CurrencyConverterComponent);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

});
