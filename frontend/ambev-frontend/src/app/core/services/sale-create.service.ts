import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Sale } from './sale.service';

@Injectable({ providedIn: 'root' })
export class SaleCreateService {
  editSaleItem(saleId: string, productId: string, model: { quantity: number; cancelled: boolean }): Observable<any> {
    return this.http.put(`${this.apiUrl}/${saleId}/item/${productId}`, model);
  }
  updateSale(id: string, sale: Partial<Sale>): Observable<Sale> {
    return this.http.put<Sale>(`${this.apiUrl}/${id}`, sale);
  }
  private apiUrl = 'http://localhost:5119/api/sales';

  constructor(private http: HttpClient) {}

  createSale(sale: Partial<Sale>): Observable<Sale> {
    return this.http.post<Sale>(this.apiUrl, sale);
  }
}
