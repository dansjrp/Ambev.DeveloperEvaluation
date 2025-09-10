import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Sale {
  id?: string;
  number: number;
  date: string;
  userId: string;
  branch: string;
  total: number;
  items: Array<{ productId: string; productName: string; quantity: number; price: number; discounts: number; totalPrice: number; cancelled: boolean }>;
}

export interface PaginatedResponse<T> {
  data: T[];
  currentPage: number;
  totalPages: number;
  totalCount: number;
}

@Injectable({ providedIn: 'root' })
export class SaleService {
  private apiUrl = 'http://localhost:5119/api/sales';

  constructor(private http: HttpClient) {}

  getSales(page = 1, size = 10, order = 'date desc'): Observable<PaginatedResponse<Sale>> {
    return this.http.get<PaginatedResponse<Sale>>(`${this.apiUrl}?page=${page}&pageSize=${size}&order=${order}`);
  }

  getSale(id: string): Observable<Sale> {
    return this.http.get<Sale>(`${this.apiUrl}/${id}`);
  }
}
